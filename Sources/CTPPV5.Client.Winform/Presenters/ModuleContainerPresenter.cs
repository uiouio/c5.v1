using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Client.Winform.Views;
using WinFormsMvp;
using CTPPV5.Infrastructure.Log;
using CTPPV5.Client.Winform.Model;
using Autofac;
using CTPPV5.Infrastructure;
using CTPPV5.Client.Winform.Views.Modules;
using CTPPV5.Infrastructure.Extension;

namespace CTPPV5.Client.Winform.Presenters
{
    public interface IModuleContainerPresenter : IPresenter<IModuleContainerView>
    {
        void OnModuleLoad(object sender, EventArgs e);
    }

    public class ModuleContainerPresenter : Presenter<IModuleContainerView>, IModuleContainerPresenter
    {
        public ModuleContainerPresenter(IModuleContainerView view)
            : base(view)
        {
            view.Load += OnModuleLoad;
        }

        public void OnModuleLoad(object sender, EventArgs e)
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                foreach (var moduleId in UserContext.Get().AuthorizedModules)
                {
                    var module = scope.ResolveOptionalKeyed<IModule>(
                        moduleId.ToEnum<ModuleType>(),
                        new NamedParameter("parent", View.ModuleContainer));
                    if (module != null)
                    {
                        View.OnModuleAdd(module);
                    }
                }
            }
            View.OnLoad();
        }

        private ILog Log { get; set; }
    }
}
