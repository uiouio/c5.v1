using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Core.Filterchain;
using Mina.Core.Session;

namespace CTPPV5.Rpc.Net.Client.Filter
{
    public class AutoAuthenticationFilter : IoFilterAdapter
    {
        public override void SessionClosed(INextFilter nextFilter, IoSession session)
        {
            //retry to reconnect and auth.
            base.SessionClosed(nextFilter, session);
        }
    }
}
