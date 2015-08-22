using CTPPV5.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Extension;

namespace CTPPV5.Security.Impl
{
    public class IdentifierProvider : IIdentifierProvider
    {
        static string identifier = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("ctppv5_command_server")).ToHex();
        public string GetIdentifier()
        {
            return identifier;
        }
    }
}
