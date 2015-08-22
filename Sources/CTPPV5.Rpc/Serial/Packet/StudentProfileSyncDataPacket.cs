using CTPPV5.Models.CommandModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Serial.Packet
{
    public class StudentProfileSyncDataPacket : DataPacket
    {
        private HolderProfile profile;
        public StudentProfileSyncDataPacket(HolderProfile profile)
            : base(profile.DestinationAddr) 
        {
            this.profile = profile;
        }
        protected override byte DataLength { get { return Convert.ToByte(5 + profile.NameBytes.Length); } }

        protected override byte Protocol { get { return 0x11; } }

        protected override void FillData(Mina.Core.Buffer.IoBuffer buffer)
        {
            buffer.Put(Convert.ToByte(profile.No % 1000));
            buffer.Put(Convert.ToByte(profile.No / 1000 << 4 + profile.No / 100 % 10));
            buffer.Put(profile.NameBytes);
            buffer.Put(profile.Month);
            buffer.Put(profile.Day);
            buffer.Put(Convert.ToByte(profile.NameBytes.Length + 2));
        }
    }
}
