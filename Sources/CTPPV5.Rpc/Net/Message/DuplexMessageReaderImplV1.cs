using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Core.Buffer;
using Autofac;
using CTPPV5.Infrastructure;
using CTPPV5.Infrastructure.Extension;
using CTPPV5.Rpc.Net.Message.Filter;
using CTPPV5.Rpc.Net.Message.Serializer;
using CTPPV5.Infrastructure.Log;

namespace CTPPV5.Rpc.Net.Message
{
    /// <summary>
    /// Read message from request
    /// <remarks>
    /// format: len(4)|version(1)|commandcode(2)|errorcode(1)|type(1)|identifier(16)|messageid(16)|filtertype(1)|filtercode(2)|serializer(1)|crc(4)|body
    /// </remarks>
    /// </summary>
    public class DuplexMessageReaderImplV1 : IMessageReader<DuplexMessage>
    {
        public int HeaderLength { get { return 49; } }

        public DuplexMessage Read(IoBuffer input)
        {
            DuplexMessage message = null;
            byte[] identifier = null;
            byte[] messageId = null;
            try
            {
                using (var scope = ObjectHost.Host.BeginLifetimeScope())
                {
                    // skip 4 bytes length 
                    input.Skip(4);
                    var filters = new List<IMessageFilter>();
                    var version = input.Get().ToEnum<MessageVersion>();
                    var commandCode = input.GetInt16().ToEnum<CommandCode>();
                    var errorCode = input.Get().ToEnum<ErrorCode>();
                    var messageType = input.Get().ToEnum<MessageType>();
                    identifier = input.GetArray(16);
                    messageId = input.GetArray(16);
                    var filterType = input.Get().ToEnum<MessageFilterType>();
                    var filterCode = new byte[] { input.Get(), input.Get() };
                    var serializeMode = input.Get().ToEnum<SerializeMode>();
                    input.Position = 0;
                    var header = input.GetArray(45);

                    var checksum = input.GetArray(4);
                    var body = input.GetRemainingArray();

                    //make data stack like 
                    //- checksum
                    //- header without checksum
                    //- body
                    //- identifier
                    var dataStack = new StackMessageDataContaner();
                    dataStack.Push(identifier);
                    dataStack.Push(body);
                    dataStack.Push(header);
                    dataStack.Push(checksum);

                    FilterResult result = new FilterResult { OK = true };
                    var filterFactory = scope.Resolve<MessageFilterFactory>();
                    filters.Add(filterFactory.CreateFilter(filterCode[0], filterType & MessageFilterType.Checksum));
                    filters.Add(filterFactory.CreateFilter(filterCode[0], filterType & MessageFilterType.Crypto));
                    filters.Add(filterFactory.CreateFilter(filterCode[0], filterType & MessageFilterType.Compression));

                    foreach (var filter in filters)
                    {
                        result = filter.In(dataStack);
                        if (!result.OK) break;
                    }

                    if (result.OK)
                    {
                        message = DuplexMessage.CreateMessage(
                            new MessageHeader(
                                messageId.ToBase64(),
                                version,
                                identifier.ToHex(),
                                filterCode,
                                filterType,
                                errorCode == ErrorCode.NoError ? MessageState.Success : MessageState.Fail,
                                errorCode,
                                serializeMode,
                                commandCode,
                                messageType), dataStack.Take());
                    }
                    else
                    {
                        message = DuplexMessage.CreateMessage(
                            new MessageHeader(
                                messageId.ToBase64(),
                                version,
                                identifier.ToHex(),
                                filterCode,
                                MessageFilterType.Checksum,
                                MessageState.Fail,
                                result.Error,
                                SerializeMode.None,
                                commandCode,
                                messageType), null);
                    }
                }
            }
            catch (Exception ex)
            {
                //dont do anything, may be let requestor retry.
                Log.Error(ErrorCode.SysError.ToString(), ex);
            }
            return message;
        }

        private ILog Log { get; set; }
    }
}
