using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Filter.Codec.Demux;
using CTPPV5.Rpc.Net.Message;
using Mina.Core.Session;
using Mina.Filter.Codec;
using Mina.Core.Buffer;

namespace CTPPV5.Rpc.Net.Codec
{
    public abstract class AbstractMessageEncoder<TMessage> : IMessageEncoder<TMessage>
        where TMessage : IMessage
    {
        public void Encode(IoSession session, TMessage message, IProtocolEncoderOutput output)
        {
            var buffer = IoBuffer.Allocate(48);
            buffer.AutoExpand = true;
            if (DoEncode(buffer, message)) 
            {
                buffer.Flip();
                output.Write(buffer);
            }
        }

        public void Encode(IoSession session, object message, IProtocolEncoderOutput output)
        {
            Encode(session, (TMessage)message, output);
        }

        protected abstract bool DoEncode(IoBuffer output, TMessage message);
    }
}
