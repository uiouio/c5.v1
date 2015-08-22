using CTPPV5.Infrastructure;
using CTPPV5.Infrastructure.Collections.ProducerConsumer;
using CTPPV5.Rpc.Serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Client.Daemon.Instruction;
using CTPPV5.Models.CommandModel;

namespace CTPPV5.Client.Daemon.Command
{
    public class SizeAllocCommand : AbstractInstructionCommand
    {
        public SizeAllocCommand(
            ChunkedProducerConsumer<IInstruction> instructionProducer)
            : base(instructionProducer) { }

        public override void OnNotifyInstructionComplete(Rpc.Serial.IInstruction instruction)
        {
            throw new NotImplementedException();
        }

        protected override Rpc.Serial.IInstruction BuildInstruction(Rpc.Net.Message.DuplexMessage commandMessage)
        {
            var instrSet = new InstructionSet(this);
            var sizeAlloc = commandMessage.GetContent<SizeAlloc>();
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                instrSet.AddInstruction(scope.Resolve<SizeAllocInstruction>(
                    new NamedParameter("command", this),
                    new NamedParameter("parameter", sizeAlloc)));
                foreach (var unit in sizeAlloc.GradeUnits)
                {
                    instrSet.AddInstruction(scope.Resolve<ClassroomProfileSyncInstruction>(
                        new NamedParameter("command", this),
                        new NamedParameter("parameter", unit)));
                }
            }
            return instrSet;
        }
    }
}
