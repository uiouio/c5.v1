using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Token { get; set; }
        public UserGroup Group { get; set; }
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public DateTime LastLoginTime { get; set; }
        public int[] AuthorizedModules { get; set; }
    }
}
