using CTPPV5.Rpc.Serial.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Serial
{
    public interface IInstruction
    {
        string Name { get; }
        InstructionState State { get; }
        ISerialPacketDecoder CreateDecoder();
        void Handle(ISerialPacket packet);
        void Emit();
    }
}
