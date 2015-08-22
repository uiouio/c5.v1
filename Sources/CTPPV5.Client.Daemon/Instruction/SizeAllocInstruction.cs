using CTPPV5.Client.Daemon.Command;
using CTPPV5.Models.CommandModel;
using CTPPV5.Rpc.Serial.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Client.Daemon.Instruction
{
    public class SizeAllocInstruction : AbstractInstruction
    {
        public SizeAllocInstruction(SizeAllocCommand command, SizeAlloc parameter)
            : base(command, parameter) { }
        public override string Name { get { return "SizeAlloc"; } }

        public override Rpc.Serial.ISerialPacketDecoder CreateDecoder()
        {
            throw new NotImplementedException();
        }

        protected override int Timeout { get { return 30 * 1000; } }

        protected override Rpc.Serial.Packet.ISerialPacket BuildPacket(object parameter)
        {
            return new SizeAllocDataPacket(parameter as SizeAlloc);
        }

        protected override void DoHandle(Rpc.Serial.Packet.ISerialPacket packet)
        {
            throw new NotImplementedException();
        }
    }
}
