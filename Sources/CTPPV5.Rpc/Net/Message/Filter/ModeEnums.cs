using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Message.Filter
{
    public enum ChecksumMode
    {
        Crc32 = 0
    }

    public enum CryptoMode
    {
        RSA = 0
    }

    public enum ZipMode
    {
        GZip = 0
    }
}
