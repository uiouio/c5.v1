using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Collections.ProducerConsumer;
using CTPPV5.Infrastructure.Security;
using CTPPV5.Rpc.Net.Message;
using Mina.Core.Session;

namespace CTPPV5.Rpc.Net.Command
{
    public abstract class AbstractBlockCommand : AbstractAsyncCommand
    {
        private DuplexMessage result;
        private SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
        public AbstractBlockCommand(
            IoSession session,
            CommandCode commandCode,
            TimeoutNotifyProducerConsumer<AbstractAsyncCommand> producer)
            :base(session, commandCode, producer)
        {
            Callback += BlockCommand_Callback;
        }

        void BlockCommand_Callback(object sender, CommandEventArgs<DuplexMessage> e)
        {
            result = e.Message;
            semaphore.Release();
        }

        public override DuplexMessage Run(int timeout = AbstractAsyncCommand.BLOCK_UNTIL_TIMEOUT_AFTER_SECONDS * 1000)
        {
            DisableRetry();
            RunAsync();
            if (!semaphore.Wait(timeout))
            {
                result = DuplexMessage.CreateAckMessage(ID, Version, CommandCode, ErrorCode.CommandRunTimeout);
                semaphore.Dispose();
            }
            return result;
        }
    }
}
