using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Models.Api
{
    public class SchoolListClassifiedRS : ApiResponse
    {
        public IList<Province> Provinces { get; set; }
    }
}
