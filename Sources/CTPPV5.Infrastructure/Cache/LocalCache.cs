using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Collections;

namespace CTPPV5.Infrastructure.Cache
{
    public class LocalCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private LRUMap<TKey, ExpirableValue<TValue>> lru;
        public LocalCache(int capacity)
        {
            this.lru = new LRUMap<TKey, ExpirableValue<TValue>>(capacity);
        }

        #region ICache<TKey,TValue> Members

        public bool TrySet(TKey key, TValue value)
        {
            return TrySet(key, value, TimeSpan.MaxValue);
        }

        public bool TrySet(TKey key, TValue value, TimeSpan expire)
        {
            return lru.TryAdd(key, new ExpirableValue<TValue>(value, expire));
        }

        public bool TryGet(TKey key, out TValue value)
        {
            var got = false;
            value = default(TValue);
            ExpirableValue<TValue> val = null;
            if (lru.TryGet(key, out val))
            {
                if (!val.IsExpired)
                {
                    val.Delay();
                    got = true;
                }
                else Remove(key);
            }
            return got;
        }

        public TValue GetOrAdd(TKey key, Func<TValue> func, TimeSpan expire)
        {
            TValue value = default(TValue);
            if (!TryGet(key, out value))
            {
                TrySet(key, func(), expire);
            }
            return value;
        }

        public void Remove(TKey key)
        {
            lru.Remove(key);
        }

        #endregion

        private class ExpirableValue<TValue>
        {
            private TimeSpan expire;
            private DateTime expireTime;
            public ExpirableValue(TValue value, TimeSpan expire)
            {
                this.Value = value;
                this.expire = expire;
                Delay();
            }

            public TValue Value { get; private set; }

            public bool IsExpired
            {
                get
                {
                    return expireTime < DateTime.Now;
                }
            }

            public void Delay()
            {
                this.expireTime = expire == TimeSpan.MaxValue ? DateTime.MaxValue : DateTime.Now.Add(expire); 
            }
        }
    }
}
