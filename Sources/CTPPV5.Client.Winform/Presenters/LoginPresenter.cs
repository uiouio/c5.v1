using CTPPV5.Client.Winform.Model;
using CTPPV5.Client.Winform.Views;
using CTPPV5.Infrastructure.Consts;
using CTPPV5.Infrastructure.Log;
using CTPPV5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMvp;
using CTPPV5.Infrastructure.Extension;
using CTPPV5.Infrastructure;
using Autofac;
using CTPPV5.Client.Winform.Api;
using CTPPV5.Models.Api;

namespace CTPPV5.Client.Winform.Presenters
{
    public interface ILoginPresenter : IPresenter<ILoginView>
    {
        void OnLogout(object sender, EventArgs e);
        void OnLogin(object sender, EventArgs e);
    }

    public class LoginPresenter : Presenter<ILoginView>, ILoginPresenter
    {
        public LoginPresenter(ILoginView view) : base(view) 
        {
            view.LoginClicked += OnLogin;
            view.Load += OnLoad;
        }

        public void OnLoad(object sender, EventArgs e)
        {
 
        }

        public void OnLogin(object sender, EventArgs e)
        {
            LoginResult result = null;
            try
            {
                using (var scope = ObjectHost.Host.BeginLifetimeScope())
                {
                    var loginRS = scope.Resolve<IRemoteApi>().Login(
                        new LoginRQ
                        {
                            Account = View.Model.AsDynamic().Account,
                            Password = View.Model.AsDynamic().Password
                        });
                    if (loginRS.Ok)
                    {
                        result = LoginResult.Success(loginRS.User);
                        loginRS.User.Bind();
                    }
                    else result = LoginResult.Fail(Constants.INVALID_ACCOUNT_OR_PASSWORD);
                }
            }
            catch(Exception ex)
            {
                Log.Error(Constants.LOGIN_ERROR, ex);
                result = LoginResult.Fail(Constants.LOGIN_ERROR);
            }

            View.Model = result;
            View.OnLoginFinished();
        }

        public void OnLogout(object sender, EventArgs e)
        {
 
        }

        private ILog Log { get; set; }
    }
}
