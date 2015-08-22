using CTPPV5.Client.Daemon.Command;
using CTPPV5.Rpc.Serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Client.Daemon.Instruction
{
    public class CheckInReplyInstruction : AbstractInstruction
    {
        public CheckInReplyInstruction(object parameter)
            : base(null, parameter) { }

        public override string Name
        {
            get { throw new NotImplementedException(); }
        }

        public override ISerialPacketDecoder CreateDecoder()
        {
            throw new NotImplementedException();
        }

        protected override Rpc.Serial.Packet.ISerialPacket BuildPacket(object parameter)
        {
            throw new NotImplementedException();
        }

        protected override void DoHandle(Rpc.Serial.Packet.ISerialPacket packet)
        {
            throw new NotImplementedException();
        }

        protected override int Timeout { get { return 0; } }
    }
}
