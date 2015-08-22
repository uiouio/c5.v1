using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Message.Filter
{
    public class Empty : IMessageFilter
    {
        static Empty filter = new Empty();
        static FilterResult result = new FilterResult { OK = true, Error = ErrorCode.NoError };
        public FilterResult In(IMessageDataContainer container)
        {
            return result;
        }

        public FilterResult Out(MessageHeader header, IMessageDataContainer container)
        {
            return result;
        }

        public static Empty Instance { get { return filter; } }
    }
}
