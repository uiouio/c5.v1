using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Message.Filter
{
    [Flags]
    public enum MessageFilterType
    {
        None = 0,
        Crypto = 1,
        Checksum = 2,
        Compression = 4,
        All = Compression | Checksum | Crypto 
    }
}
