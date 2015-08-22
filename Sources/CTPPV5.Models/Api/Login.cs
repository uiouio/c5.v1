using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Models.Api
{
    public class LoginRQ : ApiRequest
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }

    public class LoginRS : ApiResponse
    {
        public User User { get; set; }
    }
}
