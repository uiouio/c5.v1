using CTPPV5.Infrastructure.Collections.ProducerConsumer;
using CTPPV5.Infrastructure.Security;
using CTPPV5.Rpc.Net.Message;
using Mina.Core.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Rpc.Net.Command;

namespace CTPPV5.Rpc.Net.Client.Command
{
    public class Authentication : AbstractBlockCommand
    {
        public Authentication(
            IoSession session,
            TimeoutNotifyProducerConsumer<AbstractAsyncCommand> producer)
            : base(session, CommandCode.Authentication, producer)
        { }
    }
}
