using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using Mina.Core.Buffer;
using Mina.Core.Service;
using Mina.Core.Session;
using CTPPV5.Infrastructure.Extension;

namespace CTPPV5.Rpc.Server.Test
{
    public class SimpleTcpClient
    {
        private FutureHandler handler;
        private AutoResetEvent are = new AutoResetEvent(false);
        public SimpleTcpClient()
        {
            Client = new TcpClient();
            Client.SendTimeout = Convert.ToInt32(TimeSpan.FromMinutes(2).TotalMilliseconds);
            Client.ReceiveTimeout = Convert.ToInt32(TimeSpan.FromMinutes(2).TotalMilliseconds);
        }

        public TcpClient Client { get; private set; }
        public bool Connect(System.Net.IPEndPoint endpoint)
        {
            Client.Connect(endpoint);
            return Client.Connected;
        }

        public FutureHandler GetFutureHandler(
            Action<IoSession, Exception> caughtAction = null, 
            Action<IoSession, object> rcvAction = null)
        {
            handler = new FutureHandler(are, caughtAction, rcvAction);
            return handler;
        }

        public bool Send(IoBuffer output)
        {
            try
            {
                output.Flip();
                var buffer = output.GetRemainingArray();
                Client.Client.Send(buffer);
            }
            catch (Exception) { return false; }
            if (handler != null)
                return handler.Await();
            return true;
        }

        public IoBuffer Receive()
        {
            int got = 0;
            int ret = 0;
            int off = 0;

            var len = new byte[4];
            Client.Client.Receive(len);
            var buf = ByteBufferAllocator.Instance.Wrap(len);
            buf.AutoExpand = true;
            buf.Clear();
            var rcvLength = buf.GetInt32() - 4;
            var packet = new byte[rcvLength];
            while (got < rcvLength)
            {
                ret = Client.Client.Receive(packet, off + got, rcvLength - got, SocketFlags.None);
                if (ret <= 0)
                {
                    throw new Exception("tcp client read error");
                }
                got += ret;
            }
            buf.Put(packet);
            buf.Clear();
            return buf;
        }

        public void Close()
        {
            Client.Close();
        }

        public class FutureHandler : IoHandlerAdapter
        {
            private int rcvCount = 0;
            private int errorCount = 0;
            private AutoResetEvent signal;
            private Action<IoSession, Exception> caughtAction;
            private Action<IoSession, object> rcvAction;
            public FutureHandler(AutoResetEvent signal, Action<IoSession, Exception> caughtAction = null, Action<IoSession, object> rcvAction = null)
            {
                this.signal = signal;
                this.caughtAction = caughtAction;
                this.rcvAction = rcvAction;
            }
            public override void ExceptionCaught(IoSession session, Exception cause)
            {
                if (caughtAction != null)
                {
                    try
                    {
                        caughtAction(session, cause);
                    }
                    catch(Exception){}
                }
                Interlocked.Increment(ref errorCount);
                signal.Set();
            }

            public override void MessageReceived(IoSession session, object message)
            {
                if (rcvAction != null)
                {
                    try
                    {
                        rcvAction(session, message);
                    }
                    catch (Exception) 
                    {
                    }
                }
                Interlocked.Increment(ref rcvCount);
                signal.Set();
            }

            public bool Await()
            {
                return signal.WaitOne();
            }

            public int RcvCount { get { return rcvCount; } }
            public int ErrorCount { get { return errorCount; } }
        }
    }
}
