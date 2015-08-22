using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Models;

namespace CTPPV5.Client.Winform.Model
{
    public class LoginResult
    {
        private LoginResult(string message)
        {
            this.Message = message;
            this.Result = Winform.Result.Failure;
        }

        private LoginResult(User user)
        {
            this.User = user;
            this.Result = Winform.Result.Success;
        }

        public static LoginResult Fail(string message)
        {
            return new LoginResult(message);
        }

        public static LoginResult Success(User user)
        {
            return new LoginResult(user);
        }

        public User User { get; private set; }
        public Result Result { get; private set; }
        public string Message { get; private set; }
    }
}
