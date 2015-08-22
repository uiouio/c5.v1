using CTPPV5.Infrastructure.Consts;
using Mina.Transport.Serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Serial
{
    public class SerialSessionOpenException : Exception
    {
        public SerialSessionOpenException(SerialEndPoint endpoint)
            : this(endpoint, null) { }

        public SerialSessionOpenException(SerialEndPoint endpoint, Exception inner)
            : base(string.Format(ExceptionMessage.FAILED_TO_OPEN_REMOTE_ENDPOINT, endpoint.ToString()), inner) { }
    }
}
