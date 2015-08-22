using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Security
{
    public interface IIdentifierProvider
    {
        string GetIdentifier();
    }
}
