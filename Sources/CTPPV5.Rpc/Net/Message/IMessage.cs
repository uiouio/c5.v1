using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Message
{
    public interface IMessage
    {
        MessageHeader Header { get; }
        T GetContent<T>();
        byte[] GetContentBinary();
    }
}
