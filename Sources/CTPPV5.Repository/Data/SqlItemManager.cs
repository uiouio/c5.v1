using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Consts;

namespace CTPPV5.Repository.Data
{
    public class SqlItemManager
    {
        private Dictionary<string, SqlItem> itemMap;
        public SqlItemManager()
        {
            itemMap = new Dictionary<string, SqlItem>();
        }

        public int ItemCount { get { return itemMap.Count; } }

        public SqlItem Get(string sqlName)
        {
            SqlItem item = null;
            itemMap.TryGetValue(sqlName, out item);
            return item;
        }

        public bool Add(string sqlName, SqlItem item)
        {
            var addOk = false;
            if (!itemMap.ContainsKey(sqlName))
            {
                itemMap.Add(sqlName, item);
                addOk = true;
            }
            return addOk;
        }
    }
}
