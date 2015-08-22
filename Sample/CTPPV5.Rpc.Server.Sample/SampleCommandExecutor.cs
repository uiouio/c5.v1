using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Rpc.Net.Command;
using CTPPV5.TestLib;

namespace CTPPV5.Rpc.Server.Sample
{
    public class SampleCommandExecutor : AbstractCommandExecutor
    {
        protected override DuplexMessage DoExecute(DuplexMessage commandMessage)
        {
            Log.Error(ErrorCode.BadProtocalVersion, new Exception("error"));
            var content = commandMessage.GetContent<Customer>();
            return DuplexMessage.CreateCallbackMessage(commandMessage, content);
        }
    }
}
