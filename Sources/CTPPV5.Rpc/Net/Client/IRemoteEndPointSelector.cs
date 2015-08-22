using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Client
{
    public interface IRemoteEndPointSelector
    {
        bool TryPick(out IPEndPoint endpoint);
        void MarkDown(IPEndPoint endpoint);
    }
}
