using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Rpc.Net.Command;

namespace CTPPV5.Rpc.Net.Server.Command
{
    public class Heartbeat : AbstractCommandExecutor
    {
        protected override DuplexMessage DoExecute(DuplexMessage commandMessage)
        {
            commandMessage.Header.MessageType = MessageType.Heartbeat;
            return commandMessage;
        }
    }
}
