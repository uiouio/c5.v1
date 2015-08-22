using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Security;

namespace CTPPV5.Security.Impl
{
    public class RSACryptoKeyProvider : ICryptoKeyProvider
    {
        #region ICryptoKeyProvider Members

        public byte[] GetPrivateKey(string keyId)
        {
            return new byte[] { 1 };
        }

        public byte[] GetPublicKey(string keyId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
