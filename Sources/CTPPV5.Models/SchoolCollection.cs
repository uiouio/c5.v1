using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Models
{
    public class SchoolCollection : IEnumerable<School>
    {
        private Dictionary<string, School> schoolMap;
        public SchoolCollection()
        {
            schoolMap = new Dictionary<string, School>();
        }

        public School ByID(int schoolId)
        {
            School school = null;
            schoolMap.TryGetValue(string.Format("ID_{0}", schoolId), out school);
            return school;
        }

        public School ByIdentifier(string identifier)
        {
            School school = null;
            schoolMap.TryGetValue(string.Format("DENTI_{0}", identifier), out school);
            return school;
        }

        public School ByName(string name)
        {
            School school = null;
            schoolMap.TryGetValue(string.Format("Name_{0}", name), out school);
            return school;
        }

        public void Add(School school)
        {
            schoolMap[string.Format("ID_{0}", school.ID)] = school;
            schoolMap[string.Format("DENTI_{0}", school.Identifier)] = school;
            schoolMap[string.Format("Name_{0}", school.Name)] = school;
        }

        public void Clear()
        {
            schoolMap.Clear();
        }

        public IEnumerator<School> GetEnumerator()
        {
            foreach (var kvp in schoolMap)
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
