using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Models.CommandModel;

namespace CTPPV5.Rpc.Serial.Packet
{
    public class ClassSyncDataPacket : DataPacket
    {
        private GradeProfileSyncUnit gradeProfile;
        private byte ONE_BYTE_GRADE = 1;
        private byte ONE_BYTE_NAME_LEN = 1;
        public ClassSyncDataPacket(byte destinationAddr, GradeProfileSyncUnit gradeProfile)
            : base(destinationAddr)
        {
            this.gradeProfile = gradeProfile;
        }

        protected override byte DataLength
        {
            get 
            {
                return Convert.ToByte(
                    ONE_BYTE_GRADE +
                    gradeProfile.ClassUnits.Count +
                    gradeProfile.ClassUnits.Count * ONE_BYTE_NAME_LEN +
                    gradeProfile.ClassUnits.Sum(c => c.NameBytes.Length));
            }
        }

        protected override byte Protocol { get { return 0x14; } }

        protected override void FillData(Mina.Core.Buffer.IoBuffer buffer)
        {
            buffer.Put(gradeProfile.Number);
            foreach (var unit in gradeProfile.ClassUnits)
            {
                buffer.Put(Convert.ToByte(unit.Number / 10 * 16 + unit.Number % 10));
                buffer.Put(Convert.ToByte(unit.NameBytes.Length));
                buffer.Put(unit.NameBytes);
            }
        }
    }
}
