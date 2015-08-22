using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Infrastructure.Collections.ProducerConsumer;
using CTPPV5.Infrastructure.Security;
using CTPPV5.Rpc.Net.Command;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Rpc.Net.Client.Command;
using Mina.Core.Session;

namespace CTPPV5.Rpc.Net.Client
{
    public class ClientModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new AsyncSessionFactoryImpl()).AsSelf().As<IAsyncSessionFactory<DuplexMessage>>().SingleInstance();
            builder.Register((c, p) => new AsyncSessionImpl(p.Named<int>("heartbeatTimeout"), p.Named<int>("writerIdleTime"))).AsSelf().As<IAsyncSession<DuplexMessage>>();
            builder.Register((c, p) => new Authentication(
                p.Named<IoSession>("session"),
                c.ResolveOptional<TimeoutNotifyProducerConsumer<AbstractAsyncCommand>>()
                )).Keyed<AbstractAsyncCommand>(CommandCode.Authentication);
            builder.Register((c, p) => new Register(
                p.Named<IoSession>("session"),
                c.ResolveOptional<TimeoutNotifyProducerConsumer<AbstractAsyncCommand>>()
                )).Keyed<AbstractAsyncCommand>(CommandCode.Register);
            builder.Register((c, p) => new ListConnectors(
                p.Named<IoSession>("session"),
                c.ResolveOptional<TimeoutNotifyProducerConsumer<AbstractAsyncCommand>>()
                )).Keyed<AbstractAsyncCommand>(CommandCode.ListConnectors);
        }
    }
}
