using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Core.Session;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Infrastructure.Log;

namespace CTPPV5.Rpc.Net.Command
{
    public abstract class AbstractCommandExecutor : ICommandExecutor<DuplexMessage>
    {
        private IoSession session;
        public void Execute(IoSession session, DuplexMessage commandMessage)
        {
            this.session = session;
            DuplexMessage result = null;
            try
            {
                result = DoExecute(commandMessage);
            }
            catch (Exception ex)
            {
                result = DuplexMessage.CreateAckMessage(commandMessage, ErrorCode.SysError);
                Log.Error(ErrorCode.SysError.ToString(), ex);
            }
            if (result != null) Return(result);
        }

        public void Return(DuplexMessage resultMessage)
        {
            try
            {
                if (session != null) session.Write(resultMessage);
            }
            catch (Exception ex)
            {
                Log.Error(ErrorCode.SysError, ex);
            }
        }

        protected abstract DuplexMessage DoExecute(DuplexMessage commandMessage);
        protected ILog Log { get; set; }
    }
}
