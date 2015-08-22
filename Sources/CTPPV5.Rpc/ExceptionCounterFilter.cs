using Mina.Core.Filterchain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Core.Session;
using CTPPV5.Infrastructure.Consts;
using CTPPV5.Infrastructure.Log;
using CTPPV5.Rpc.Net.Message;

namespace CTPPV5.Rpc
{
    public class ExceptionCounterFilter : IoFilterAdapter
    {
        const int MAX_UNCAUGHT_SESSION_ERROR_COUNT = 5;
        public override void ExceptionCaught(INextFilter nextFilter, IoSession session, Exception cause)
        {
            Log.Error(ErrorCode.UnhandledExceptionCaught, cause);
            var counter = session.GetAttribute<int>(KeyName.SESSION_ERROR_COUNTER);
            if (counter >= MAX_UNCAUGHT_SESSION_ERROR_COUNT) session.Close(true);
            else
            {
                session.SetAttribute(KeyName.SESSION_ERROR_COUNTER, ++counter);
                base.ExceptionCaught(nextFilter, session, cause);
            }
        }

        public override void MessageReceived(INextFilter nextFilter, IoSession session, object message)
        {
            base.MessageReceived(nextFilter, session, message);
            session.SetAttribute(KeyName.SESSION_ERROR_COUNTER, 0);
        }

        private ILog Log { get; set; }
    }
}
