using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.CommandService.Test
{
    public class SimpleCommandServerBootstrap
    {
        private string ip;
        private int port;
        public SimpleCommandServerBootstrap()
        {
            port = new Random().Next(1000, 8000);
            ip = "127.0.0.1";
        }

        public IPEndPoint EndPoint { get { return new IPEndPoint(IPAddress.Parse(ip), port); } }

        public CommandServer Server { get; private set; }

        public bool Start()
        {
            var config = new ServerConfig
            {
                Port = port,
                Ip = ip,
                Mode = SocketMode.Tcp,
                Name = "CommandServer"
            };
            Server = new CommandServer();
            if (Server.Setup(config))
                return Server.Start();
            else return false;
        }
    }
}
