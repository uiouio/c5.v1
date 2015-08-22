using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Message
{
    public enum MessageType
    {
        Command = 1,
        CommandAck = 2,
        Callback = 3,
        Heartbeat = 4
    }
}
