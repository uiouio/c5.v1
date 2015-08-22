using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Rpc;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Rpc.Net.Command;
using CTPPV5.Client.Daemon.Instruction;
using CTPPV5.Infrastructure.Collections.ProducerConsumer;
using CTPPV5.Rpc.Serial;

namespace CTPPV5.Client.Daemon.Command
{
    public abstract class AbstractInstructionCommand : AbstractCommandExecutor
    {
        private ChunkedProducerConsumer<IInstruction> instructionProducer;
        public const int INSTRUCTION_QUEUE_CAPACITY = 1000;
        protected AbstractInstructionCommand(ChunkedProducerConsumer<IInstruction> instructionProducer)
        {
            this.instructionProducer = instructionProducer;
        }

        protected override DuplexMessage DoExecute(DuplexMessage commandMessage)
        {
            var instruction = BuildInstruction(commandMessage);
            if (instruction != null)
            {
                instructionProducer.Produce(instruction);
                return DuplexMessage.CreateAckMessage(commandMessage);
            }
            else return DuplexMessage.CreateAckMessage(commandMessage, ErrorCode.InstructionBuildFailed);
        }

        public abstract void OnNotifyInstructionComplete(IInstruction instruction);
        protected abstract IInstruction BuildInstruction(DuplexMessage commandMessage);
    }
}
