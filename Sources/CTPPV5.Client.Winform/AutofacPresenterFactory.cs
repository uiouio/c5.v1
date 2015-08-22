using CTPPV5.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMvp;
using WinFormsMvp.Binder;
using Autofac;

namespace CTPPV5.Client.Winform
{
    public class AutofacPresenterFactory : IPresenterFactory
    {
        public WinFormsMvp.IPresenter Create(Type presenterType, Type viewType, IView viewInstance)
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                return scope.Resolve(presenterType, new NamedParameter("view", viewInstance)) as IPresenter;
            }
        }

        public void Release(IPresenter presenter)
        {
       
        }
    }
}
