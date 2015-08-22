using CTPPV5.Client.Daemon.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Rpc.Serial.Packet;
using CTPPV5.Rpc.Serial;
using CTPPV5.Infrastructure.Extension;

namespace CTPPV5.Client.Daemon.Instruction
{
    public class CheckInQueryInstruction : AbstractInstruction
    {
        public CheckInQueryInstruction(CheckInQueryCommand command, object parameter)
            : base(command, parameter) { }

        protected override ISerialPacket BuildPacket(object parameter)
        {
            return new CheckInQueryCtlPacket(parameter.AsDynamic().DestinationAddr);
        }

        public override ISerialPacketDecoder CreateDecoder()
        {
            throw new NotImplementedException();
        }

        public override string Name { get { return "CheckInQuery"; } }
        protected override int MaxRetryCount { get { return 0; } }

        protected override void DoHandle(ISerialPacket packet)
        {
            throw new NotImplementedException();
        }
        protected override int Timeout { get { return 300; } }
        public override int Delay { get { return 300; } }
    }
}
