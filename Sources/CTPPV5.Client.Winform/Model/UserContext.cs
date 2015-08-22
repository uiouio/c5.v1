using CTPPV5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Client.Winform.Model
{
    public static class UserContext
    {
        const string CONTEXT_NAME = "UserContext_User";
        public static User Get()
        {
            User user = null;
            var data = AppDomain.CurrentDomain.GetData(CONTEXT_NAME);
            if (data != null)
                user = data as User;
            return user;
        }

        public static void Bind(this User user)
        {
            AppDomain.CurrentDomain.SetData(CONTEXT_NAME, user);
        }

        public static void Unbind(this User user)
        {
            var getUser = Get();
            if (getUser != null)
                AppDomain.CurrentDomain.SetData(CONTEXT_NAME, null);
        }
    }
}
