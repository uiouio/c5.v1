using Mina.Core.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Message
{
    public interface IMessageWriter<TMessage> where TMessage : IMessage
    {
        bool Write(IoBuffer output, TMessage message);
    }
}
