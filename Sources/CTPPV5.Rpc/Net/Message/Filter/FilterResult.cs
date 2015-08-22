using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Message.Filter
{
    public class FilterResult
    {
        public bool OK { get; set; }
        public ErrorCode Error { get; set; }
    }
}
