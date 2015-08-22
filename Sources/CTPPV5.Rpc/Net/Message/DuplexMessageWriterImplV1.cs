using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Infrastructure;
using Mina.Core.Buffer;
using CTPPV5.Rpc.Net.Message.Filter;
using CTPPV5.Infrastructure.Extension;
using CTPPV5.Infrastructure.Log;

namespace CTPPV5.Rpc.Net.Message
{
    /// <summary>
    /// write message to response
    /// <remarks>
    /// format: len(4)|version(1)|commandcode(2)|errorcode(1)|type(1)|identifier(16)|messageid(16)|filtertype(1)|filtercode(2)|serializer(1)|crc(4)|body
    /// </remarks>
    /// </summary>
    public class DuplexMessageWriterImplV1 : IMessageWriter<DuplexMessage>
    {
        public bool Write(IoBuffer output, DuplexMessage message)
        {
            var writeOk = false;
            try
            {
                using (var scope = ObjectHost.Host.BeginLifetimeScope())
                {
                    var filters = new List<IMessageFilter>();
                    var dataStack = new StackMessageDataContaner();
                    var filterFactory = scope.Resolve<MessageFilterFactory>();
                    filters.Add(filterFactory.CreateFilter(
                        message.Header.FilterCode[0], message.Header.FilterType & MessageFilterType.Compression));
                    filters.Add(filterFactory.CreateFilter(
                        message.Header.FilterCode[0], message.Header.FilterType & MessageFilterType.Crypto));
                    var identifierBinary = message.Header.Identifier.FromHex();
                    dataStack.Push(identifierBinary);
                    dataStack.Push(message.GetContentBinary());
                    FilterResult result = new FilterResult { OK = true };
                    foreach (var filter in filters)
                    {
                        result = filter.Out(message.Header, dataStack);
                        if (!result.OK) break;
                    }
                    if (result.OK)
                    {
                        var body = dataStack.Take();
                        output.PutInt32(49 + body.Length);
                        output.Put(message.Header.Version.ToByte());
                        output.PutInt16(Convert.ToInt16(message.Header.CommandCode));
                        output.Put(message.Header.ErrorCode.ToByte());
                        output.Put(message.Header.MessageType.ToByte());
                        output.Put(identifierBinary);
                        output.Put(message.Header.MessageID.FromBase64());
                        output.Put(message.Header.FilterType.ToByte());
                        output.Put(message.Header.FilterCode);
                        output.Put(message.Header.SerializeMode.ToByte());
                        output.Flip();
                        var header = output.GetRemainingArray();
                        dataStack.Push(body);
                        dataStack.Push(header);
                        result = filterFactory
                            .CreateFilter(message.Header.FilterCode[0],
                                message.Header.FilterType & MessageFilterType.Checksum)
                            .Out(message.Header, dataStack);
                        writeOk = result.OK;
                        if (writeOk)
                        {
                            output.Put(dataStack.Take());
                            output.Put(body);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ErrorCode.SysError.ToString(), ex);
            }
            return writeOk;
        }

        private ILog Log { get; set; }
    }
}
