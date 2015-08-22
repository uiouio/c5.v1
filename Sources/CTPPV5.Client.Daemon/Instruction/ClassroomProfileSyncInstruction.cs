using CTPPV5.Client.Daemon.Command;
using CTPPV5.Models.CommandModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Rpc.Serial.Packet;

namespace CTPPV5.Client.Daemon.Instruction
{
    public class ClassroomProfileSyncInstruction : AbstractInstruction
    {
        public ClassroomProfileSyncInstruction(SizeAllocCommand command, GradeProfileSyncUnit parameter)
            : base(command, parameter) { }

        public override string Name { get { return "ClassProfileSyncInstruction"; } }

        public override Rpc.Serial.ISerialPacketDecoder CreateDecoder()
        {
            throw new NotImplementedException();
        }

        protected override int Timeout { get { return 10 * 1000; } }

        protected override Rpc.Serial.Packet.ISerialPacket BuildPacket(object parameter)
        {
            var gradeSyncUnit = parameter as GradeProfileSyncUnit;
            return new ClassSyncDataPacket(gradeSyncUnit.DestinationAddr, gradeSyncUnit);
        }

        protected override void DoHandle(Rpc.Serial.Packet.ISerialPacket packet)
        {
            throw new NotImplementedException();
        }
    }
}
