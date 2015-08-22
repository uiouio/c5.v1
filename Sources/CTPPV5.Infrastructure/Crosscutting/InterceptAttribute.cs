using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Crosscutting
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AutoChangeLogAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class AutoLocalCacheAttribute : Attribute
    {
 
    }
}
