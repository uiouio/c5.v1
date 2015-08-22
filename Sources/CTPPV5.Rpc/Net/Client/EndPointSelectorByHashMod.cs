using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace CTPPV5.Rpc.Net.Client
{
    public class EndPointSelectorByHashMod : IRemoteEndPointSelector
    {
        private IList<IPEndPoint> endpointList;
        public EndPointSelectorByHashMod(IPEndPoint[] endpoints)
        {
            endpointList = endpoints.ToList();
        }

        public bool TryPick(out IPEndPoint endpoint)
        {
            var picked = false;
            endpoint = null;
            if (endpointList.Count > 0)
            {
                endpoint = endpointList[Math.Abs(Guid.NewGuid().ToString().GetHashCode() % endpointList.Count)];
            }
            return picked;
        }

        public void MarkDown(IPEndPoint endpoint)
        {
            for (var i = endpointList.Count - 1; i > 0; i--)
            {
                if (endpointList[i].ToString().Equals(endpoint.ToString()))
                {
                    endpointList.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
