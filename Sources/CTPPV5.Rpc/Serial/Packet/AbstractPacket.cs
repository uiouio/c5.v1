using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Core.Buffer;

namespace CTPPV5.Rpc.Serial.Packet
{
    public abstract class AbstractPacket : ISerialPacket
    {
        private byte destinationAddr;
        protected const byte FRAME_SEQUENCE_VALUE = 0;
        protected const byte FRAME_SOURCE_ADDR = 0;
        protected AbstractPacket(byte destinationAddr)
        {
            this.destinationAddr = destinationAddr;
        }
        public IoBuffer GetBuffer()
        {
            var buf = new ByteBufferAllocator().Allocate(Length);
            buf.AutoExpand = false;
            buf.Put(Symbol);
            buf.Put(Symbol);
            buf.Put(destinationAddr);
            this.Fill(buf);
            buf.Put(ComputeChecksum(buf));
            return buf;
        }
        protected abstract byte Symbol { get; }
        protected abstract byte Length { get; }
        protected abstract void Fill(IoBuffer buffer);
        private byte ComputeChecksum(IoBuffer buffer)
        {
            buffer.Rewind();
            byte checksum = 0;
            for (int i = 0; i < Length - 1; i++) checksum += buffer.Get();
            return checksum;
        }
    }
}
