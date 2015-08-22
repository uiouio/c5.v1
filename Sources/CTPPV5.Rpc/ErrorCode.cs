using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc
{
    public enum ErrorCode
    {
        NoError = 0,
        BadProtocalVersion = 1,
        DataBroken = 2,
        DecryptFailed = 3,
        EncryptFailed = 4,
        CryptoKeyReadFailed = 5,
        UnzipFailed = 6,
        UnhandledExceptionCaught = 7,
        SessionCloseError = 8,
        MessageRetryError = 9,
        CommandRunTimeout = 10,
        IllegalCommandRequest = 11,
        UnauthorizedCommand = 12,
        AuthenticationFailed = 13,
        RegisterFailed = 14,
        BadRequest = 15,
        FireCommandError = 16,
        InstructionBuildFailed = 17,
        SerialPortSessionOpenError = 501,
        InstructionHandleError = 502,
        SysError = 999
    }
}
