using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Domain
{
    public class School
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string DatabaseName { get; set; }
        public byte[] ServerPrivateKey { get; set; }
        public byte[] ServerPubKey { get; set; }
        public byte[] ClientPubKey { get; set; }

        public void AddGrade()
        {
 
        }

        public void AddDept()
        {
 
        }

        public void AddAdmin()
        {
 
        }
    }
}
