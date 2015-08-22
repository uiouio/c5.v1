using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Rpc.Serial.Packet;
using Mina.Filter.Codec.Demux;
using Mina.Core.Session;
using Mina.Filter.Codec;
using Mina.Core.Buffer;

namespace CTPPV5.Rpc.Serial
{
    public class PacketEncoder : IMessageEncoder<ISerialPacket>
    {
        public void Encode(IoSession session, ISerialPacket message, IProtocolEncoderOutput output)
        {
            var buf = message.GetBuffer();
            buf.Flip();
            output.Write(buf);
        }

        public void Encode(IoSession session, object message, IProtocolEncoderOutput output)
        {
            Encode(session, (ISerialPacket)message, output);
        }
    }
}
