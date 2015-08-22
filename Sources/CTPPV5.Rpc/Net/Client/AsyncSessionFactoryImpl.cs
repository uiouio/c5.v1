using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Autofac;
using Mina.Transport.Socket;
using Mina.Core.Session;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Infrastructure.Extension;
using CTPPV5.Infrastructure.Util;
using CTPPV5.Infrastructure;
using CTPPV5.Infrastructure.Consts;
using System.Text.RegularExpressions;

namespace CTPPV5.Rpc.Net.Client
{
    public class AsyncSessionFactoryImpl : IAsyncSessionFactory<DuplexMessage>
    {
        private DoubleCheckLock doubleCheckLock;
        private ConcurrentDictionary<string, IAsyncSession<DuplexMessage>> sessionMap;
        private object syncRoot = new object();
        private const int WRITER_IDLE_TIME = 60 * 5;
        private const int HEARTBEAT_TIMEOUT = 5;
       
        public AsyncSessionFactoryImpl()
        {
            doubleCheckLock = new DoubleCheckLock();
            sessionMap = new ConcurrentDictionary<string, IAsyncSession<DuplexMessage>>();
        }

        public IAsyncSession<DuplexMessage> OpenSession()
        {
            IPEndPoint endpoint = null;
            IAsyncSession<DuplexMessage> session = null;
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                var selector = scope.Resolve<IRemoteEndPointSelector>();
                while (selector.TryPick(out endpoint))
                {
                    try
                    {
                        session = OpenSession(new ConnectionConfig(endpoint));
                        if (!session.Connected)
                        {
                            selector.MarkDown(endpoint);
                            session = null;
                        }
                        else break;
                    }
                    catch (Exception)
                    {
                        selector.MarkDown(endpoint);
                    }
                }
                if (session == null)
                    throw new SessionOpenException(endpoint);
                return session;
            }
        }

        /// <summary>
        /// open session with connectionstring
        /// </summary>
        /// <param name="connectionString">
        /// format: 127.0.0.1:1433;timeout=5;keepalive=true;
        /// </param>
        /// <returns></returns>
        public IAsyncSession<DuplexMessage> OpenSession(string connectionString)
        {
            ConnectionConfig config = null;
            if (!ConnectionConfig.TryParse(connectionString, out config))
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessage.INVALID_SESSION_CONNECT_STRING, connectionString));
            }
            return OpenSession(config);
        }

        public IAsyncSession<DuplexMessage> OpenSession(IoSession ioSession)
        {
            IAsyncSession<DuplexMessage> session = null;
            using (var locker = doubleCheckLock.Accquire(
                () => sessionMap.TryGetValue(ioSession.RemoteEndPoint.ToString(), out session)))
            {
                if (locker.Locked)
                {
                    using (var scope = ObjectHost.Host.BeginLifetimeScope())
                    {
                        session = scope.Resolve<IAsyncSession<DuplexMessage>>(
                            new NamedParameter("heartbeatTimeout", HEARTBEAT_TIMEOUT),
                            new NamedParameter("writerIdleTime", WRITER_IDLE_TIME));
                        if (session.Open(ioSession))
                            sessionMap[ioSession.RemoteEndPoint.ToString()] = session;
                        else
                            throw new SessionOpenException(ioSession.RemoteEndPoint as IPEndPoint);
                    }
                }
                else
                {
                    sessionMap.TryRemove(ioSession.RemoteEndPoint.ToString(), out session);
                    throw new SessionOpenException(ioSession.RemoteEndPoint as IPEndPoint);
                }
            }

            return session;
        }

        private IAsyncSession<DuplexMessage> OpenSession(ConnectionConfig config)
        {
            IAsyncSession<DuplexMessage> session = null;
            using (var locker = doubleCheckLock.Accquire(
                () => !sessionMap.TryGetValue(config.EndPoint.ToString(), out session)))
            {
                if (locker.Locked)
                {
                    using (var scope = ObjectHost.Host.BeginLifetimeScope())
                    {
                        session = scope.Resolve<IAsyncSession<DuplexMessage>>(
                            new NamedParameter("heartbeatTimeout", HEARTBEAT_TIMEOUT),
                            new NamedParameter("writerIdleTime", WRITER_IDLE_TIME));
                        session.Open(config);
                        sessionMap[config.EndPoint.ToString()] = session;
                    }
                }
                else
                {
                    if (!session.Connected)
                    {
                        sessionMap.TryRemove(config.EndPoint.ToString(), out session);
                        session = OpenSession(config);
                    }
                }
            }

            return session;
        }
    }
}
