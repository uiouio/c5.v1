using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Crosscutting
{
    public class AttributeLookup 
    {
        private ConcurrentDictionary<MemberInfo, HashSet<Type>> attributeMap;
        public AttributeLookup()
        {
            attributeMap = new ConcurrentDictionary<MemberInfo, HashSet<Type>>();
        }

        public bool IsDefined<TAttribute>(MemberInfo info)
        {
            var isDefined = false;
            HashSet<Type> set = null;
            if (!attributeMap.TryGetValue(info, out set))
            {
                set = new HashSet<Type>();
                foreach (var attr in info.GetCustomAttributes())
                {
                    if (!set.Contains(attr.GetType()))
                        set.Add(attr.GetType());
                }
                attributeMap[info] = set;
            }
            isDefined = set.Contains(typeof(TAttribute));
            return isDefined;
        }
    }
}
