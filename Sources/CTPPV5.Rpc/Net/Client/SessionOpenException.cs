using CTPPV5.Infrastructure.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Client
{
    public class SessionOpenException : Exception
    {
        public SessionOpenException(IPEndPoint endpoint)
            : this(endpoint, null) { }

        public SessionOpenException(IPEndPoint endpoint, Exception inner)
            : base(string.Format(ExceptionMessage.FAILED_TO_OPEN_REMOTE_ENDPOINT, endpoint.ToString()), inner) { }
    }
}
