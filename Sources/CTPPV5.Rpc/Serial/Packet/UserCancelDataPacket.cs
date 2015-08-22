using CTPPV5.Models.CommandModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Serial.Packet
{
    public class UserCancelDataPacket : DataPacket
    {
        private UserCancel userCancel;
        public UserCancelDataPacket(UserCancel userCancel)
            : base(userCancel.DestinationAddr) 
        {
            this.userCancel = userCancel;
        }

        protected override byte DataLength { get { return 2; } }

        protected override byte Protocol { get { return 0x07; } }
        protected override void FillData(Mina.Core.Buffer.IoBuffer buffer)
        {
            buffer.Put(Convert.ToByte(userCancel.UserNo % 100));
            buffer.Put(Convert.ToByte(userCancel.UserNo / 1000 << 4 + userCancel.UserNo / 100 % 10));
        }
    }
}
