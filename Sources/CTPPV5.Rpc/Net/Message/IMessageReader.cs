using Mina.Core.Buffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Message
{
    /// <summary>
    /// thread unsafe interface.
    /// when implement this interface, you should have to consider a none state share design.
    /// </summary>
    public interface IMessageReader<TMessage> where TMessage : IMessage
    {
        int HeaderLength { get; }
        TMessage Read(IoBuffer input);
    }
}
