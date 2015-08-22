using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Core.Buffer;

namespace CTPPV5.Rpc.Serial.Packet
{
    public abstract class CtlPacket : AbstractPacket
    {
        private const byte DEFAULT_SYMBOL = (byte)'*';
        private const int PACKET_LENGTH = 7;
        protected CtlPacket(byte destinationAddr)
            : base(destinationAddr) { }

        public static bool True(byte[] symbol)
        {
            return symbol.SequenceEqual(new byte[] { DEFAULT_SYMBOL, DEFAULT_SYMBOL });
        }

        protected override byte Symbol { get { return DEFAULT_SYMBOL; } }
        protected override byte Length { get { return PACKET_LENGTH; } }
        protected override void Fill(IoBuffer buffer)
        {
            buffer.Put(FRAME_SOURCE_ADDR);
            buffer.Put(Respond);
            buffer.Put(FRAME_SEQUENCE_VALUE);
        }
        protected abstract byte Respond { get; }
    }
}
