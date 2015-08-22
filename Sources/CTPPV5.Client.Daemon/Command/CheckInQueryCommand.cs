using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CTPPV5.Repository;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Infrastructure.Collections.ProducerConsumer;
using CTPPV5.Client.Daemon.Instruction;
using CTPPV5.Rpc.Serial;
using CTPPV5.Infrastructure;
using Autofac;

namespace CTPPV5.Client.Daemon.Command
{
    public class CheckInQueryCommand : AbstractInstructionCommand
    {
        private const int LOCAL_SCHOOL_ID = 0;
        private IHardwareRepository hardwareRepository;
        private AutoResetEvent are;
        private static readonly DuplexMessage message;

        static CheckInQueryCommand()
        {
            message = DuplexMessage.CreateCommandMessage(string.Empty, MessageVersion.V1, CommandCode.Test);
        }

        public CheckInQueryCommand(
            ChunkedProducerConsumer<IInstruction> instructionProducer,
            IHardwareRepository hardwareRepository)
            : base(instructionProducer)
        {
            this.are = new AutoResetEvent(false);
            this.hardwareRepository = hardwareRepository;
        }

        public void Execute()
        {
            new Task(() =>
            {
                while (true)
                {
                    foreach (var hardware in hardwareRepository
                        .GetHardwares(LOCAL_SCHOOL_ID))
                    {
                        base.Execute(null, DuplexMessage.CreateCommandMessage(
                            string.Empty,
                            MessageVersion.V1,
                            CommandCode.Test,
                            Rpc.Net.Message.Filter.MessageFilterType.None,
                            new byte[0],
                            Rpc.Net.Message.Serializer.SerializeMode.None,
                            new { DestinationAddr = hardware.Address }));
                        are.WaitOne(500);
                    }
                }
            }).Start();
        }

        protected override IInstruction BuildInstruction(DuplexMessage commandMessage)
        {
            var instrSet = new InstructionSet(this);
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                instrSet.AddInstruction(scope.Resolve<CheckInQueryInstruction>(
                    new NamedParameter("command", this),
                    new NamedParameter("parameter", commandMessage.GetContent<object>())));
                instrSet.AddInstruction(scope.Resolve<CheckInReplyInstruction>(
                    new NamedParameter("parameter", commandMessage.GetContent<object>())));
            }
            return instrSet;
        }

        public override void OnNotifyInstructionComplete(IInstruction instruction)
        {
            are.Set();
        }
    }
}
