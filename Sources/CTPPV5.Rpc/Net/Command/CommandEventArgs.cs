using CTPPV5.Rpc.Net.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Command
{
    public class CommandEventArgs<TMessage> : EventArgs
        where TMessage : IMessage
    {
        public CommandEventArgs(TMessage message)
        {
            this.Message = message;
        }

        public TMessage Message { get; private set; }
    }
}
