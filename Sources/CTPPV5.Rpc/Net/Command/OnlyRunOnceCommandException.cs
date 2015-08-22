using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Consts;

namespace CTPPV5.Rpc.Net.Command
{
    public class OnlyRunOnceCommandException : Exception
    {
        public OnlyRunOnceCommandException(AbstractBlockCommand command)
            : base(string.Format(ExceptionMessage.BLOCK_COMMAND_CAN_ONLY_RUN_ONCE_ERROR, command.CommandCode.ToString(), command.SessionIdentifier)) { }
    }
}
