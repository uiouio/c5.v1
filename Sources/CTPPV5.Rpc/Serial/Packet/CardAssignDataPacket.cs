using CTPPV5.Models.CommandModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Serial.Packet
{
    public class CardAssignDataPacket : DataPacket
    {
        private CardAssignUnit assignUnit;
        public CardAssignDataPacket(CardAssignUnit unit)
            : base(unit.DestinationAddr) 
        {
            this.assignUnit = unit;
        }

        protected override byte DataLength { get { return 7; } }

        protected override byte Protocol { get { return 0x0d; } }

        protected override void FillData(Mina.Core.Buffer.IoBuffer buffer)
        {
            buffer.Put(Convert.ToByte(assignUnit.HolderNo % 100));
            buffer.Put(Convert.ToByte(assignUnit.HolderNo / 1000 << 4 + assignUnit.HolderNo / 100 % 10));
            buffer.Put(Convert.ToByte((assignUnit.No & 0xff000000) >> 24));
            buffer.Put(Convert.ToByte((assignUnit.No & 0x00ff0000) >> 16));
            buffer.Put(Convert.ToByte((assignUnit.No & 0x0000ff00) >> 8));
            buffer.Put(Convert.ToByte(assignUnit.No & 0x000000ff));
            buffer.Put(assignUnit.Sequence);
        }
    }
}
