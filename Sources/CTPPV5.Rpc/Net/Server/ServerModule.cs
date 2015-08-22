using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Rpc.Net.Command;
using CTPPV5.Rpc.Net.Server.Filter;
using CTPPV5.Rpc.Net.Server.Command;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Infrastructure.Module;

namespace CTPPV5.Rpc.Net.Server
{
    public class ServerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new MetricsModule());
            builder.Register(c => new SessionIdentifierManager()).AsSelf().SingleInstance();
            builder.Register(c => new SessionAbnormalFilter(c.Resolve<SessionIdentifierManager>())).AsSelf().SingleInstance();
            builder.Register(c => new BadRequest()).Keyed<ICommandExecutor<DuplexMessage>>(CommandCode.BadRequest);
            builder.Register(c => new BadRequest()).Keyed<ICommandExecutor<DuplexMessage>>(CommandCode.Test);
            builder.Register(c => new Heartbeat()).Keyed<ICommandExecutor<DuplexMessage>>(CommandCode.Heartbeat);
        }
    }
}
