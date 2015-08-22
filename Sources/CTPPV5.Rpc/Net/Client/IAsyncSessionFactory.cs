using CTPPV5.Rpc.Net.Message;
using Mina.Core.Session;
using Mina.Transport.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Client
{
    public interface IAsyncSessionFactory<TMessage> where TMessage : IMessage
    {
        IAsyncSession<TMessage> OpenSession();
        IAsyncSession<TMessage> OpenSession(IoSession ioSession);
        IAsyncSession<TMessage> OpenSession(string connectionString);
    }
}
