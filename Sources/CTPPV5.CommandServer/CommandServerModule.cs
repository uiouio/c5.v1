using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.CommandServer.Command;
using CTPPV5.Rpc.Net.Command;
using CTPPV5.Repository;
using CTPPV5.Rpc.Net.Message;
using Autofac.Core;
using CTPPV5.Rpc.Net.Server;

namespace CTPPV5.CommandServer
{
    public class CommandServerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<AuthenticationValidationFilter>()
                .WithParameters(new ResolvedParameter[]
                {
                    new ResolvedParameter((pi, ctx) => pi.ParameterType == typeof(SessionIdentifierManager), (pi, ctx) => ctx.Resolve<SessionIdentifierManager>()),
                    new ResolvedParameter((pi, ctx) => pi.ParameterType == typeof(IMetaRepository), (pi, ctx) => ctx.Resolve<IMetaRepository>()), 
                })
                .SingleInstance();
            builder.Register(c => new Register(c.Resolve<IMetaRepository>())).Keyed<ICommandExecutor<DuplexMessage>>(CommandCode.Register);
            builder.Register(c => new ListConnectors(c.Resolve<SessionIdentifierManager>())).Keyed<ICommandExecutor<DuplexMessage>>(CommandCode.ListConnectors);
        }
    }
}
