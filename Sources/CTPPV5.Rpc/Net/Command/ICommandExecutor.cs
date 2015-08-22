using CTPPV5.Rpc.Net.Message;
using Mina.Core.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Command
{
    public interface ICommandExecutor<TMessage>
        where TMessage : IMessage
    {
        void Execute(IoSession session, TMessage commandMessage);
    }
}
