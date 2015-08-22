using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Models
{
    public class Area
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public SchoolCollection Schools { get; set; }
    }
}
