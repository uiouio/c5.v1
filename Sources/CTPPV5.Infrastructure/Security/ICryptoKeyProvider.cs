using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Security
{
    public interface ICryptoKeyProvider
    {
        byte[] GetPrivateKey(string keyId);
        byte[] GetPublicKey(string keyId);
    }
}
