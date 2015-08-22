using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CTPPV5.CommandService.Test
{
    public class SimpleCommandServerClient
    {
        private TcpClient client;
        private IPEndPoint endPoint;
        public SimpleCommandServerClient(IPEndPoint endPoint)
        {
            this.endPoint = endPoint;
            this.client = new TcpClient();
        }

        public bool Connect()
        {
            client.Connect(endPoint);
            //server is async
            Thread.Sleep(500);
            return client.Connected;
        }

        public bool Close()
        {
            return false;
        }
    }
}
