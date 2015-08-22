using CTPPV5.Rpc.Net.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Command
{
    public interface IAsyncCommand<TCallBackMessage> : ICommand<TCallBackMessage>
        where TCallBackMessage : IMessage
    {
        string ID { get; }
        int Timeout { get; set; }
        void RunAsync();
        void RaiseCallback(object sender, CommandEventArgs<TCallBackMessage> args);
        event EventHandler<CommandEventArgs<TCallBackMessage>> Callback;
    }
}
