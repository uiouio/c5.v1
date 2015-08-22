using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Infrastructure;
using CTPPV5.Infrastructure.Collections.ProducerConsumer;
using CTPPV5.Rpc.Serial;
using CTPPV5.Client.Daemon.Command;
using CTPPV5.Repository;
using CTPPV5.Infrastructure.Module;
using CTPPV5.Client.Daemon.Instruction;
using CTPPV5.Rpc;
using CTPPV5.Models.CommandModel;

namespace CTPPV5.Client.Daemon
{
    public class DaemonModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new LogModule());
            builder.RegisterModule(new SecurityModule());
            builder.RegisterModule(new RepositoryModule());
            builder.Register(c => new ExceptionCounterFilter()).AsSelf().SingleInstance();
            builder.Register(c => new SerialShell()).AsSelf().SingleInstance();
            builder.Register(c => new PacketHandler()).AsSelf().SingleInstance();
            builder.RegisterType<ChunkedProducerConsumer<IInstruction>>()
               .WithParameter(new NamedParameter("capacity", AbstractInstructionCommand.INSTRUCTION_QUEUE_CAPACITY))
               .AsSelf()
               .SingleInstance()
               .OnActivated(o =>
               {
                   o.Instance.OnConsume += (sender, e) =>
                       {
                           var consumingItem = e.ConsumingItem as ChunkedConsumingItem<IInstruction>;
                           if (consumingItem == null) return;
                           foreach (var item in consumingItem.Chunk)
                           {
                               try
                               {
                                   item.Emit();
                               }
                               catch (Exception) { }
                           }
                       };
               });

            builder.Register(c => new CheckInQueryCommand(c.Resolve<ChunkedProducerConsumer<IInstruction>>(), c.Resolve<IHardwareRepository>()));
            builder.Register(c => new AddrAssignCommand(c.Resolve<ChunkedProducerConsumer<IInstruction>>()));
            builder.Register(c => new SizeAllocCommand(c.Resolve<ChunkedProducerConsumer<IInstruction>>()));
            builder.Register(c => new CardAssignCommand(c.Resolve<ChunkedProducerConsumer<IInstruction>>()));
            builder.Register(c => new TimeSyncCommand(c.Resolve<ChunkedProducerConsumer<IInstruction>>()));
            builder.Register(c => new CardLogoutCommand(c.Resolve<ChunkedProducerConsumer<IInstruction>>()));
            builder.Register(c => new UserCancelCommand(c.Resolve<ChunkedProducerConsumer<IInstruction>>()));
            builder.Register((c, p) => 
                new CheckInQueryInstruction(p.Named<CheckInQueryCommand>("command"), p.Named<object>("parameter")))
                .As<IInstruction>().AsSelf();
            builder.Register((c, p) =>
                new CheckInReplyInstruction(p.Named<object>("parameter")))
                .As<IInstruction>().AsSelf();
            builder.Register((c, p) =>
                new AddrAssignInstruction(p.Named<AddrAssignCommand>("command"), p.Named<AddrAssign>("parameter")))
                .As<IInstruction>().AsSelf();
            builder.Register((c, p) =>
                new SizeAllocInstruction(p.Named<SizeAllocCommand>("command"), p.Named<SizeAlloc>("parameter")))
                .As<IInstruction>().AsSelf();
            builder.Register((c, p) =>
                new ClassroomProfileSyncInstruction(p.Named<SizeAllocCommand>("command"), p.Named<GradeProfileSyncUnit>("parameter")))
                .As<IInstruction>().AsSelf();
            builder.Register((c, p) =>
                new CardAssignInstruction(p.Named<CardAssignCommand>("command"), p.Named<CardAssignUnit>("parameter")))
                .As<IInstruction>().AsSelf();
            builder.Register((c, p) =>
                new StudentProfileSyncInstruction(p.Named<CardAssignCommand>("command"), p.Named<HolderProfile>("parameter")))
                .As<IInstruction>().AsSelf();
            builder.Register((c, p) =>
                new TimeSyncInstruction(p.Named<TimeSyncCommand>("command"), p.Named<byte>("parameter")))
                .As<IInstruction>().AsSelf();
            builder.Register((c, p) =>
                new CardLogoutInstruction(p.Named<CardLogoutCommand>("command"), p.Named<CardAssignUnit>("parameter")))
                .As<IInstruction>().AsSelf();
            builder.Register((c, p) =>
                new UserCancelInstruction(p.Named<UserCancelCommand>("command"), p.Named<UserCancel>("parameter")))
                .As<IInstruction>().AsSelf();
        }
    }
}
