using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Models
{
    public class HardwareCollection : IEnumerable<Hardware>
    {
        public Dictionary<string, Hardware> hardwareMap;
        public HardwareCollection()
        {
            this.hardwareMap = new Dictionary<string, Hardware>();
        }

        public Hardware ByAddress(int address)
        {
            Hardware hardware = null;
            hardwareMap.TryGetValue(string.Format("Addr_{0}", address), out hardware);
            return hardware;
        }

        public void Add(Hardware hardware)
        {
            hardwareMap[string.Format("Addr_{0}", hardware.Address)] = hardware;
        }

        public IEnumerator<Hardware> GetEnumerator()
        {
            foreach (var val in hardwareMap.Values)
                yield return val;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
