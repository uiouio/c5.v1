using CTPPV5.Rpc.Net.Message;
using CTPPV5.Rpc.Net.Message.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Command
{
    public interface ICommand<TMessage>
        where TMessage : IMessage
    {
        CommandCode CommandCode { get; }
        MessageVersion Version { get; set; }
        object Parameter { get; set; }
        bool SecurityEnabled { get; set; }
        SerializeMode SerializeMode { get; set; }
        TMessage Run(int timeout);
    }
}
