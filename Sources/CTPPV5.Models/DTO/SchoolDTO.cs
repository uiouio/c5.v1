using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Models.DTO
{
    public class SchoolDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string DatabaseName { get; set; }
        public string ServerPrivateKey { get; set; }
        public string ServerPubKey { get; set; }
        public string ClientPubKey { get; set; }
    }
}
