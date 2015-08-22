using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Rpc.Net.Command;
using CTPPV5.Rpc.Net.Message;
using Mina.Core.Session;

namespace CTPPV5.Rpc.Net.Client
{
    public interface IAsyncSession<TMessage> where TMessage : IMessage
    {
        bool Connected { get; }
        void Open(ConnectionConfig config);
        bool Open(IoSession ioSession);
        void Close();
        IAsyncCommand<TMessage> CreateCommand(CommandCode code);
    }
}
