using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Models.Api;

namespace CTPPV5.Client.Winform.Api
{
    public interface IRemoteApi
    {
        LoginRS Login(LoginRQ loginRQ);
        SchoolListClassifiedRS GetSchoolListClassified(ApiRequest apiRQ);
    }
}
