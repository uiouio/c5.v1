using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Cache
{
    public interface ICache<TKey, TValue>
    {
        bool TrySet(TKey key, TValue value, TimeSpan expire);
        bool TryGet(TKey key, out TValue value);
        TValue GetOrAdd(TKey key, Func<TValue> func, TimeSpan expire);
        void Remove(TKey key);
    }
}
