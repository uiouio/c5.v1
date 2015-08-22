using Mina.Core.Service;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Mina.Core.Session;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Infrastructure.Collections.ProducerConsumer;
using CTPPV5.Infrastructure.Consts;
using CTPPV5.Infrastructure.Collections;
using CTPPV5.Infrastructure;

namespace CTPPV5.Rpc.Net.Command
{
    public class CommandHandler : IoHandlerAdapter
    {
        private TimeoutNotifyProducerConsumer<AbstractAsyncCommand> producer;
        public CommandHandler(TimeoutNotifyProducerConsumer<AbstractAsyncCommand> producer)
        {
            this.producer = producer;
        }

        public override void MessageReceived(IoSession session, object message)
        {
            var duplexMessage = message as DuplexMessage;
            if (duplexMessage != null)
            {
                switch (duplexMessage.Header.MessageType)
                {
                    case MessageType.Command:
                        {
                            using (var scope = ObjectHost.Host.BeginLifetimeScope())
                            {
                                scope.ResolveKeyed<ICommandExecutor<DuplexMessage>>(
                                    duplexMessage.Header.CommandCode).Execute(session, duplexMessage);
                            }
                        }
                        break;
                    default:
                        {
                            var messageId = duplexMessage.Header.MessageID;
                            AbstractAsyncCommand command = null;
                            //var callbackMap = session
                            //    .GetAttribute<LRUMap<string, AbstractAsyncCommand>>(KeyName.SESSION_COMMAND_CALLBACK);
                            var tryToRemoveOk = producer.TryRemoveThoseWaitToTimeout(messageId, out command);
                            if (duplexMessage.Header.MessageType == MessageType.CommandAck)
                            {
                                if (tryToRemoveOk)
                                {
                                    command.Parameter = null;
                                    if (duplexMessage.Header.State == MessageState.Success)
                                        producer.Produce(messageId, command, command.Timeout);
                                    else
                                        command.RaiseCallback(command, new CommandEventArgs<DuplexMessage>(duplexMessage));
                                }
                            }
                            else
                            {
                                //if (!tryToRemoveOk)
                                //    if (callbackMap.TryGet(messageId, out command))
                                //        callbackMap.Remove(messageId);

                                if (command != null)
                                    command.RaiseCallback(command, new CommandEventArgs<DuplexMessage>(duplexMessage));
                            }
                        }
                        break;
                }
            }
        }
    }
}
