using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Util;

namespace CTPPV5.Infrastructure.Extension
{
    public static class ObjectExtension
    {
        public static dynamic AsDynamic<T>(this T @object)
        {
            return new TransparentObject<T>(@object);
        }
    }
}
