using CTPPV5.Models.CommandModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Serial.Packet
{
    public class SizeAllocDataPacket : DataPacket
    {
        private SizeAlloc alloc;
        public SizeAllocDataPacket(SizeAlloc alloc)
            : base(alloc.DestinationAddr)
        {
            this.alloc = alloc;
        }
        
        protected override byte Protocol { get { return 0x0a; } }

        protected override void FillData(Mina.Core.Buffer.IoBuffer buffer)
        {
            foreach (var unit in alloc.Units)
            {
                buffer.Put(Convert.ToByte(unit.Address / 10 * 16 + unit.Address % 10));
                buffer.Put(unit.Size);
            }
            buffer.Put(Convert.ToByte(91 / 10 * 16 + 91 % 10));
            buffer.Put((byte)70);
            buffer.Put(Convert.ToByte(92 / 10 * 16 + 92 % 10));
            buffer.Put((byte)70);
            buffer.Put(Convert.ToByte(93 / 10 * 16 + 93 % 10));
            buffer.Put((byte)70);
            buffer.Put(Convert.ToByte(94 / 10 * 16 + 94 % 10));
            buffer.Put((byte)70);
            buffer.Put(Convert.ToByte(95 / 10 * 16 + 95 % 10));
            buffer.Put((byte)70);
        }

        protected override byte DataLength { get { return Convert.ToByte((alloc.Units.Count + 5) * 2); } }
    }
}
