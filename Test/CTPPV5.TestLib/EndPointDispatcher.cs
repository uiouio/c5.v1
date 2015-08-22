using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.TestLib
{
    public class EndPointDispatcher
    {
        static int counter = 9000;
        public static IPEndPoint GetRandomPort()
        {
            return new IPEndPoint(IPAddress.Parse("127.0.0.1"), System.Threading.Interlocked.Increment(ref counter));
        }
    }
}
