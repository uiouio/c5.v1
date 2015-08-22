using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Infrastructure.Security;
using CTPPV5.TestLib;
using CTPPV5.Rpc.Net.Command;
using Mina.Core.Session;
using CTPPV5.Infrastructure.Collections.ProducerConsumer;
using CTPPV5.Rpc.Net.Message;

namespace CTPPV5.Rpc.Client.Sample
{
    public class SampleModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new TestRsaKeyProvider()).As<ICryptoKeyProvider>();
            builder.Register(c => new TestIdentityProvider()).As<IIdentifierProvider>();
            builder.Register((c, p) => new Heartbeat(
                p.Named<IoSession>("session"),
                c.ResolveOptional<TimeoutNotifyProducerConsumer<AbstractAsyncCommand>>()
                )).Keyed<AbstractAsyncCommand>(CommandCode.Heartbeat);
        }

        public class Heartbeat : AbstractBlockCommand
        {
            public Heartbeat(
            IoSession session,
            TimeoutNotifyProducerConsumer<AbstractAsyncCommand> producer)
                : base(session, CommandCode.Heartbeat, producer) { }
        }
    }
}
