using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using CTPPV5.Rpc.Serial;
using CTPPV5.Client.Daemon.Command;

namespace CTPPV5.Client.Daemon.Instruction
{
    public class InstructionSet : IInstruction
    {
        private IList<IInstruction> instrs;
        private AbstractInstructionCommand command;

        public InstructionSet(AbstractInstructionCommand command)
        {
            this.instrs = new List<IInstruction>();
            this.command = command;
        }

        public InstructionState State { get; private set; }

        public ISerialPacketDecoder CreateDecoder()
        {
            throw new NotImplementedException();
        }

        public void Handle(Rpc.Serial.Packet.ISerialPacket packet)
        {
            throw new NotImplementedException();
        }

        public void AddInstruction(IInstruction instr)
        {
            instrs.Add(instr);
        }

        public void Emit()
        {
            State = InstructionState.Success;
            foreach (var instr in instrs)
            {
                instr.Emit();
                if (instr.State != InstructionState.Success)
                {
                    State = InstructionState.Failure;
                    break;
                }
            }
            command.OnNotifyInstructionComplete(this);
        }

        public string Name { get { return "InstructionSet"; } }

        public int Timeout { get; set; }
    }
}
