using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Consts;
using Mina.Core.Filterchain;
using Mina.Core.Session;

namespace CTPPV5.Rpc.Net.Server.Filter
{
    public class SessionAbnormalFilter : IoFilterAdapter
    {
        private SessionIdentifierManager identifierManager;
        public SessionAbnormalFilter(SessionIdentifierManager identifierManager)
        {
            this.identifierManager = identifierManager;
        }

        public override void SessionIdle(INextFilter nextFilter, IoSession session, IdleStatus status)
        {
            if (status == IdleStatus.ReaderIdle) session.Close(true);
        }

        public override void SessionClosed(INextFilter nextFilter, IoSession session)
        {
            identifierManager.RemoveSessionId(session.GetAttribute<string>(KeyName.SESSION_IDENTIFIER));
            base.SessionClosed(nextFilter, session);
        }
    }
}
