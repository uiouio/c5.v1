using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Models
{
    public class AreaCollection : IEnumerable<Area>
    {
        private Dictionary<string, Area> areaMap;
        public AreaCollection()
        {
            areaMap = new Dictionary<string, Area>();
        }

        public Area ByID(int areaId)
        {
            Area area = null;
            areaMap.TryGetValue(string.Format("ID_{0}", areaId), out area);
            return area;
        }

        public Area ByName(string name)
        {
            Area area = null;
            areaMap.TryGetValue(string.Format("NAME_{0}", name), out area);
            return area;
        }
        public void Add(Area area)
        {
            areaMap[string.Format("ID_{0}", area.ID)] = area;
            areaMap[string.Format("NAME_{0}", area.Name)] = area;
        }

        public void Clear()
        {
            areaMap.Clear();
        }

        public IEnumerator<Area> GetEnumerator()
        {
            foreach (var kvp in areaMap)
            {
                if (kvp.Key.StartsWith("ID",
                    StringComparison.OrdinalIgnoreCase))
                    yield return kvp.Value;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
