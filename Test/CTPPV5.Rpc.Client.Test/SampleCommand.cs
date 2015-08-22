using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Collections.ProducerConsumer;
using CTPPV5.Infrastructure.Security;
using CTPPV5.Rpc.Net.Command;
using CTPPV5.Rpc.Net.Message;
using Mina.Core.Session;

namespace CTPPV5.Rpc.Client.Test
{
    public class SampleCommand : AbstractBlockCommand
    {
        public SampleCommand(
            IoSession session,
            TimeoutNotifyProducerConsumer<AbstractAsyncCommand> producer)
            :base(session, CommandCode.Test, producer)
        {
   
        }
    }
}
