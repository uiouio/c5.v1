using Mina.Core.Filterchain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net
{
    public abstract class AbstractFilter : IoFilterAdapter
    {
        public abstract string Name { get; }
    }
}
