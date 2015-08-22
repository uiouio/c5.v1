using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Core.Future;
using Mina.Core.Service;
using Mina.Core.Session;
using Mina.Filter.Codec;
using Mina.Filter.Codec.TextLine;
using Mina.Filter.Logging;
using Mina.Transport.Serial;
using Autofac;
using CTPPV5.Infrastructure;
using CTPPV5.Infrastructure.Extension;
using CTPPV5.Infrastructure.Log;
using CTPPV5.Infrastructure.Consts;

namespace CTPPV5.Rpc.Serial
{
    public class SerialShell
    {
        private IConnectFuture future;
        private SerialEndPoint endpoint;
        private SerialPortConfig config;
        public SerialShell()
        {
            this.config = new SerialPortConfig();
        }

        public bool StartUp(params Autofac.Module[] modules)
        {
            return ObjectHost.Setup(
                new Module[] 
                { 
                    new RpcSerialModule()
                }.Concat(modules));
        }

        public IoSession Open(SerialPortConfig config = null)
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                if (!IsOpened())
                {
                    try
                    {
                        config = config ?? new SerialPortConfig();
                        endpoint = new SerialEndPoint(config.PortName, config.BaudRate);
                        var serial = new SerialConnector();
                        serial.FilterChain.AddLast("logger", new LoggingFilter());
                        serial.FilterChain.AddLast("codec", new ProtocolCodecFilter(new PacketCodecFactory()));
                        serial.FilterChain.AddLast("exceptionCounter", scope.Resolve<ExceptionCounterFilter>());
                        serial.Handler = scope.Resolve<PacketHandler>();
                        serial.SessionCreated += (sender, e) =>
                        {
                            e.Session.SetAttributeIfAbsent(KeyName.SESSION_ERROR_COUNTER, 0);
                        };
                        future = serial.Connect(endpoint);
                        future.Await();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ErrorCode.SerialPortSessionOpenError, ex);
                        throw new SerialSessionOpenException(endpoint, ex);
                    }
                }
            }

            if (IsOpened()) return future.Session;
            else throw new SerialSessionOpenException(endpoint);
        }

        private bool IsOpened()
        {
            return future != null && future.Connected;
        }

        public void Close()
        {
            if (IsOpened()) future.Session.Close(true);
        }

        private ILog Log { get; set; }
    }
}
