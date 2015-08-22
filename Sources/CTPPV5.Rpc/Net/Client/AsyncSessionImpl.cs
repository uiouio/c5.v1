using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net;
using Autofac;
using Mina.Transport.Socket;
using Mina.Core.Session;
using Mina.Filter.Logging;
using Mina.Filter.Codec;
using Mina.Filter.KeepAlive;
using CTPPV5.Rpc.Net.Codec;
using CTPPV5.Infrastructure;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Infrastructure.Log;
using CTPPV5.Infrastructure.Consts;
using CTPPV5.Rpc.Net.Command;
using CTPPV5.Infrastructure.Collections;
using CTPPV5.Rpc.Net.Client.Filter;
using CTPPV5.Rpc.Net.Client.Filter.KeepAlive;

namespace CTPPV5.Rpc.Net.Client
{
    public class AsyncSessionImpl : IAsyncSession<DuplexMessage>
    {
        private int writerIdleTime;
        private int heartbeatTimeout;
        private IPEndPoint endPoint;
        private IoSession sessionImpl;
        private object syncRoot = new object();
        private AsyncSocketConnector connector;
        private const int MAX_CALLBACK_COUNT_EACH_SESSION = 500;

        public AsyncSessionImpl(int heartbeatTimeout, int writerIdleTime)
        {
            this.heartbeatTimeout = heartbeatTimeout;
            this.writerIdleTime = writerIdleTime;
        }

        public bool Connected { get { return sessionImpl != null && sessionImpl.Connected; } }

        public bool Open(IoSession ioSession)
        {
            if (!ioSession.Connected)
                return false;

            if (sessionImpl == null)
            {
                sessionImpl = ioSession;
                return Connected;
            }

            if (sessionImpl.Id == ioSession.Id)
                return Connected;
            
            Close();
            sessionImpl = ioSession;

            endPoint = ioSession.RemoteEndPoint as IPEndPoint;

            return Connected;
        }

        public void Open(ConnectionConfig config)
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                try
                {
                    this.endPoint = config.EndPoint;
                    connector = new AsyncSocketConnector();
                    connector.ConnectTimeoutInMillis = config.Timeout;
                    connector.FilterChain.AddLast("logger", new LoggingFilter());
                    connector.FilterChain.AddLast("codec",
                        new ProtocolCodecFilter(new MessageCodecFactory()));
                    if (config.KeepAlive)
                    {
                        var keepAliveFilter = new KeepAliveFilter(
                            new KeepAliveMessageFactory(), IdleStatus.WriterIdle, KeepAliveRequestTimeoutHandler.Exception, writerIdleTime, heartbeatTimeout);
                        connector.FilterChain.AddLast("keep-alive", keepAliveFilter);
                    }
                    connector.FilterChain.AddLast("exceptionCounter", scope.Resolve<ExceptionCounterFilter>());
                    connector.Handler = scope.Resolve<CommandHandler>();
                    connector.SessionCreated += (sender, e) =>
                    {
                        e.Session.SetAttributeIfAbsent(KeyName.SESSION_ERROR_COUNTER, 0);
                    };

                    var future = connector.Connect(endPoint);
                    future.Await();
                    sessionImpl = future.Session;
                }
                catch (Exception ex)
                {
                    throw new SessionOpenException(endPoint, ex);
                }
            }
        }

        public void Close()
        {
            this.Close(false);
        }

        public void Close(bool rightNow)
        {
            if (rightNow)
            {
                if (sessionImpl != null)
                    sessionImpl.Close(true);
                if (connector != null)
                    connector.Dispose();
            }
            else
            {
                //elegant close
                new Task((state) =>
                {
                    try
                    {
                        var tuple = state as Tuple<IoSession, AsyncSocketConnector>;
                        var waitCloseSession = tuple.Item1;
                        var waitCloseConnector = tuple.Item2;
                        if (waitCloseSession != null)
                        {
                            waitCloseSession.Close(false);
                            waitCloseSession.CloseFuture.Await();
                        }
                        if (waitCloseConnector != null)
                            waitCloseConnector.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ErrorCode.SessionCloseError.ToString(), ex);
                    }
                }, Tuple.Create<IoSession, AsyncSocketConnector>(sessionImpl, connector)).Start();
            }
        }

        public IAsyncCommand<DuplexMessage> CreateCommand(CommandCode code)
        {
            if (!Connected)
                throw new SessionOpenException(endPoint);

            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                lock (syncRoot)
                {
                    return scope.ResolveOptionalKeyed<AbstractAsyncCommand>(code, new NamedParameter("session", sessionImpl));
                }
            }
        }

        private ILog Log { get; set; }
    }
}
