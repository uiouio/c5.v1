using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Message
{
    public enum CommandCode
    {
        UnAssigned = 0,
        BadRequest = 1,
        [Secure]
        Register = 2,
        [Secure]
        Authentication = 3,
        ListConnectors = 4,
        Heartbeat = 100,
        Test = 255,
    }
}
