using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Consts
{
    public static class ExceptionMessage
    {
        public const string UNKNOWN_TYPE_TO_SERIALIZE = "Unknown type to serialize - {0}";
        public const string UNKNOWN_TYPE_TO_DESERIALIZE = "Unknown type to deserialize - byte[{0}] - {1}";
        public const string FAILED_TO_OPEN_REMOTE_ENDPOINT = "Failed to connect to remote end point - {0}";
        public const string INVALID_SESSION_CONNECT_STRING = "Invalid session connection string - {0}";
        public const string COMMAND_RUN_ERROR = "Command on session - {0} run error";
        public const string TOO_MUCH_RETRY = "Command on session - {0} discarded for Too much retry";
        public const string RAISE_CALLBACK_ERROR = "Rasie callback on command on session - {0} error";
        public const string BLOCK_COMMAND_CAN_ONLY_RUN_ONCE_ERROR = "Block command - {0} on session - {1} can only run once";
        public const string SQLITEM_DUPLICATE_EXCEPTION = "Duplidate sql item name - {0} found";
    }
}
