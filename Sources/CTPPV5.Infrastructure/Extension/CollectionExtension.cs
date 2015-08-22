using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Extension
{
    public static class CollectionExtension
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection != null) foreach (var item in collection) action(item);
        }
    }
}
