using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Core.Buffer;

namespace CTPPV5.Rpc.Serial.Packet
{
    public class AddrAssignDataPacket : DataPacket
    {
        private byte addrChangeTo;
        public AddrAssignDataPacket(byte destinationAddr, byte addrChangeTo)
            : base(destinationAddr) 
        {
            this.addrChangeTo = addrChangeTo;
        }

        protected override byte Protocol { get { return 0x00; } }
        protected override byte DataLength { get { return 1; } }
        protected override void FillData(IoBuffer buffer)
        {
            buffer.Put(addrChangeTo);
        }
    }
}
