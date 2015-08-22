using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CTPPV5.Rpc.Serial.Packet;
using CTPPV5.Client.Daemon.Command;
using CTPPV5.Rpc.Serial;
using CTPPV5.Infrastructure;
using Autofac;
using Mina.Core.Session;
using CTPPV5.Infrastructure.Log;
using CTPPV5.Rpc;
using System.Diagnostics;
using CTPPV5.Infrastructure.Consts;

namespace CTPPV5.Client.Daemon.Instruction
{
    public abstract class AbstractInstruction : IInstruction
    {
        private static int autoIncrIdentity = 0;
        private object parameter;
        private AbstractInstructionCommand command;
        private Stopwatch timing;
        private int instrDelay = 0;
        private const int MAX_RETRY_COUNT = 3;
        private int retryCount = 0;
        private object syncRoot = new object();
        protected AbstractInstruction(AbstractInstructionCommand command, object parameter)
        {
            this.command = command;
            this.parameter = parameter;
            this.timing = new Stopwatch();
            Interlocked.Increment(ref autoIncrIdentity);
        }
        public int ID { get { return autoIncrIdentity; } }
        /// <summary>
        /// must delay in millsecs
        /// </summary>
        public virtual int Delay { get { return instrDelay; } }

        public InstructionState State { get; private set; }

        public void Emit()
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                lock (syncRoot)
                {
                    State = InstructionState.Emit;
                    var session = scope.Resolve<SerialShell>().Open();
                    session.RemoveAttribute(KeyName.INSTRUCTION);
                    session.SetAttribute(KeyName.INSTRUCTION, this);
                    session.Write(this.BuildPacket(parameter));
                    timing.Start();
                    if (!Monitor.Wait(syncRoot, Timeout))
                    {
                        timing.Stop();
                        State = InstructionState.Failure;
                        Retry();
                    }
                }
            }
        }

        public abstract string Name { get; }
        public abstract ISerialPacketDecoder CreateDecoder();

        /// <summary>
        /// timeout in secs
        /// </summary>
        protected abstract int Timeout { get; }
        protected virtual int MaxRetryCount { get { return MAX_RETRY_COUNT; } }
        protected abstract ISerialPacket BuildPacket(object parameter);
        protected abstract void DoHandle(ISerialPacket packet);
        protected AbstractInstructionCommand Command { get { return command; } }
        public void Handle(ISerialPacket packet)
        {
            lock (syncRoot)
            {
                if (State == InstructionState.Emit)
                {
                    try
                    {
                        DoHandle(packet);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Format("{0} - {1}", ErrorCode.InstructionHandleError, Name), ex);
                    }
                    finally
                    {
                        timing.Stop();
                        var waitTime = Delay - timing.ElapsedMilliseconds;
                        if (waitTime > 0) Thread.Sleep(TimeSpan.FromMilliseconds(waitTime));
                        Monitor.Pulse(syncRoot);
                    }
                }
            }
        }

        private void Retry()
        {
            if (retryCount < MaxRetryCount)
            {
                retryCount++;
                Emit();
            }
        }

        private ILog Log { get; set; }
    }
}
