using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Rpc.Net.Message;

namespace CTPPV5.Rpc.Net.Message.Filter
{
    public interface IMessageFilter
    {
        FilterResult In(IMessageDataContainer container);
        FilterResult Out(MessageHeader header, IMessageDataContainer container);
    }
}
