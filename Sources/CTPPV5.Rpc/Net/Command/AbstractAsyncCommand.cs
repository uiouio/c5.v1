using CTPPV5.Rpc.Net.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Mina.Core.Session;
using CTPPV5.Infrastructure.Security;
using CTPPV5.Rpc.Net.Message.Filter;
using CTPPV5.Rpc.Net.Message.Serializer;
using CTPPV5.Infrastructure;
using CTPPV5.Infrastructure.Extension;
using CTPPV5.Infrastructure.Collections.ProducerConsumer;
using System.Net;
using CTPPV5.Infrastructure.Log;
using CTPPV5.Infrastructure.Consts;
using CTPPV5.Rpc.Net.Client;

namespace CTPPV5.Rpc.Net.Command
{
    public abstract class AbstractAsyncCommand : IAsyncCommand<DuplexMessage>
    {
        private int hasRunCount;
        private const int MAX_RUN_COUNT = 4; //allow to retry 3 times
        private IoSession session;
        private SerializeMode serializeMode = SerializeMode.Protobuf;
        private readonly byte[] DEFAULT_FILTER_CODE = new byte[2];
        private TimeoutNotifyProducerConsumer<AbstractAsyncCommand> producer;
        public const int BLOCK_UNTIL_TIMEOUT_QUEUE_CAPACITY = 5000;
        public const int BLOCK_UNTIL_TIMEOUT_AFTER_SECONDS = 5;

        protected AbstractAsyncCommand(
            IoSession session, 
            CommandCode commandCode,
            TimeoutNotifyProducerConsumer<AbstractAsyncCommand> producer)
        {
            this.session = session;
            this.CommandCode = commandCode;
            this.producer = producer;
            this.ID = Guid.NewGuid().ToByteArray().ToBase64();
            this.Version = MessageVersion.V1;
        }

        public string ID { get; private set; }

        public string SessionIdentifier { get { return session.GetAttribute<string>(KeyName.SESSION_IDENTIFIER); } }

        public CommandCode CommandCode { get; private set; }

        public SerializeMode SerializeMode
        {
            get { return serializeMode; }
            set { serializeMode = value; }
        }

        public int Timeout { get; set; }
        public MessageVersion Version { get; set; }
        public bool SecurityEnabled { get; set; }

        public object Parameter { get; set; }
        public event EventHandler<CommandEventArgs<DuplexMessage>> Callback;

        public void DisableRetry()
        {
            hasRunCount = MAX_RUN_COUNT;
        }

        public virtual void RunAsync()
        {
            if (!session.Connected)
                throw new SessionOpenException(session.RemoteEndPoint as IPEndPoint);

            if (hasRunCount <= MAX_RUN_COUNT)
            {
                var commandMessage = BuildMessage();
                if (producer.Produce(ID, this, BLOCK_UNTIL_TIMEOUT_AFTER_SECONDS))
                {
                    hasRunCount++;
                    try
                    {
                        session.Write(commandMessage);
                    }
                    catch (Exception ex)
                    {
                        AbstractAsyncCommand command = null;
                        producer.TryRemoveThoseWaitToTimeout(commandMessage.Header.MessageID, out command);
                        Log.Error(string.Format(ExceptionMessage.COMMAND_RUN_ERROR, session.GetAttribute<string>(KeyName.SESSION_IDENTIFIER)), ex);
                        this.RaiseCallback(this, new CommandEventArgs<DuplexMessage>(DuplexMessage.CreateAckMessage(commandMessage, ErrorCode.FireCommandError)));
                    }
                }
            }
            else
            {
                Log.Warn(
                    string.Format(ExceptionMessage.TOO_MUCH_RETRY,
                    session.GetAttribute<string>(KeyName.SESSION_IDENTIFIER)));
            }
        }

        public void RaiseCallback(object sender, CommandEventArgs<DuplexMessage> args)
        {
            try
            {
                if (Callback != null)
                    Callback(sender, args);
            }
            catch(Exception ex)
            {
                Log.Error(string.Format(ExceptionMessage.RAISE_CALLBACK_ERROR, session.GetAttribute<string>(KeyName.SESSION_IDENTIFIER)), ex);
            }
        }

        public virtual DuplexMessage Run(int timeout = -1)
        {
            throw new NotImplementedException();
        }

        protected ILog Log { get; set; }

        private DuplexMessage BuildMessage()
        {
            var filterType = MessageFilterType.Checksum | MessageFilterType.Compression;
            if (SecurityEnabled) filterType |= MessageFilterType.Crypto;
            
            if (Parameter == null)
                serializeMode = SerializeMode.None;
            else
            {
                if (BasicType.IsBasicType(Parameter.GetType()))
                    serializeMode = SerializeMode.BasicType;
            }

            return DuplexMessage
                .CreateCommandMessage(
                    ID, 
                    Version, 
                    CommandCode, 
                    filterType, 
                    DEFAULT_FILTER_CODE, 
                    serializeMode, 
                    Parameter);
        }
    }
}
