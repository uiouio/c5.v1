using CTPPV5.Infrastructure.Collections.ProducerConsumer;
using CTPPV5.Rpc.Net.Command;
using CTPPV5.Rpc.Net.Message;
using Mina.Core.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Client.Command
{
    public class ListConnectors : AbstractBlockCommand
    {
        public ListConnectors(
            IoSession session,
            TimeoutNotifyProducerConsumer<AbstractAsyncCommand> producer)
            : base(session, CommandCode.ListConnectors, producer)
        { }
    }
}
