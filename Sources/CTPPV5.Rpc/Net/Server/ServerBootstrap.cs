using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Autofac;
using Mina.Filter.Logging;
using Mina.Transport.Socket;
using Mina.Filter.Codec;
using CTPPV5.Rpc.Net.Codec;
using CTPPV5.Infrastructure;
using CTPPV5.Infrastructure.Extension;
using CTPPV5.Rpc.Net.Command;
using CTPPV5.Infrastructure.Module;
using Mina.Core.Filterchain;
using CTPPV5.Rpc.Net.Server.Filter;

namespace CTPPV5.Rpc.Net.Server
{
    public class ServerBootstrap
    {
        private const int DEFAULT_PORT = 8001;
        private AsyncSocketAcceptor acceptor;
        private IList<AbstractFilter> filters;
        private IList<Type> filterTypes;
        public ServerBootstrap()
            : this(new IPEndPoint(IPAddress.Any, DEFAULT_PORT)) { }
        public ServerBootstrap(IPEndPoint endPoint)
        {
            this.EndPoint = endPoint;
            this.filters = new List<AbstractFilter>();
            this.filterTypes = new List<Type>();
        }

        public IPEndPoint EndPoint { get; private set; }
        public AsyncSocketAcceptor Server { get { return acceptor; } }

        public void AddFilterTypes(Type filterType)
        {
            if (typeof(AbstractFilter).IsAssignableFrom(filterType))
            {
                filterTypes.Add(filterType);
            }
        }

        public void AddFilters(AbstractFilter filter)
        {
            filters.Add(filter);
        }

        public bool StartUp(ServerConfig config = null, params Autofac.Module[] modules)
        {
            var setupOk = ObjectHost.Setup(
                new Module[] 
                { 
                    new RpcNetModule(),
                    new ServerModule()
                }.Concat(modules));

            if (setupOk)
            {
                using(var scope = ObjectHost.Host.BeginLifetimeScope())
                {
                    config = config ?? new ServerConfig();
                    acceptor = new AsyncSocketAcceptor(config.MaxConnections);
                    acceptor.FilterChain.AddLast("session-metrics", new SessionMetricsFilter());
                    acceptor.FilterChain.AddLast("logger", new LoggingFilter());
                    acceptor.FilterChain.AddLast("codec",
                        new ProtocolCodecFilter(new MessageCodecFactory()));
                    filterTypes.ForEach(type => filters.Add(scope.Resolve(type) as AbstractFilter));
                    filters.ForEach(filter => acceptor.FilterChain.AddLast(filter.Name, filter));
                    acceptor.FilterChain.AddLast("exceptionCounter", scope.Resolve<ExceptionCounterFilter>());
                    acceptor.FilterChain.AddLast("session-abnormal", scope.Resolve<SessionAbnormalFilter>());
                    acceptor.Handler = scope.Resolve<CommandHandler>();
                    acceptor.SessionConfig.ReadBufferSize = config.ReadBufferSize;
                    acceptor.SessionConfig.SendBufferSize = config.SendBufferSize;
                    acceptor.SessionConfig.ReaderIdleTime = config.ReaderIdleTime;
                    acceptor.SessionConfig.WriterIdleTime = config.WriterIdleTime;
                    acceptor.SessionConfig.WriteTimeout = config.WriteTimeout;
                    acceptor.SessionConfig.KeepAlive = config.KeepAlive;
                    acceptor.Bind(EndPoint);
                }
            }
            return setupOk;
        }   
    }
}
