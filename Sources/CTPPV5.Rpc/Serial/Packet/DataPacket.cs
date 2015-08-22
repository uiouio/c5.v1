using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Core.Buffer;

namespace CTPPV5.Rpc.Serial.Packet
{
    public abstract class DataPacket : AbstractPacket
    {
        private const byte DEFAULT_SYMBOL = (byte)'@';
        protected DataPacket(byte destinationAddr)
            : base(destinationAddr) { }

        public static bool True(byte[] symbol)
        {
            return symbol.SequenceEqual(new byte[] { DEFAULT_SYMBOL, DEFAULT_SYMBOL });
        }
        protected override byte Symbol { get { return DEFAULT_SYMBOL; } }
        protected override void Fill(IoBuffer buffer)
        {
            buffer.Put(Length);
            buffer.Put(FRAME_SOURCE_ADDR);
            buffer.Put(Protocol);
            buffer.Put(FRAME_SEQUENCE_VALUE);
            this.FillData(buffer);
        }
        protected override byte Length { get { return Convert.ToByte(8 + DataLength); } }
        protected abstract byte DataLength { get; }
        protected abstract byte Protocol { get; }
        protected abstract void FillData(IoBuffer buffer);
        
    }
}
