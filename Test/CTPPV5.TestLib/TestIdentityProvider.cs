using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using CTPPV5.Infrastructure.Security;
using CTPPV5.Infrastructure.Extension;

namespace CTPPV5.TestLib
{
    public class TestIdentityProvider : IIdentifierProvider
    {
        #region IIdentifierProvider Members

        public string GetIdentifier()
        {
            return "098f6bcd4621d373cade4e832627b4f6"; //MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("test")).ToHex();
        }

        #endregion
    }
}
