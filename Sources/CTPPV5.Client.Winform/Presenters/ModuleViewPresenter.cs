using CTPPV5.Client.Winform.Views.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMvp;

namespace CTPPV5.Client.Winform.Presenters
{
    public interface IModuleViewPresenter<TView> : IPresenter<TView> where TView : class, IView
    {
        void OnActivated(object sender, EventArgs e);
    }

    public abstract class ModuleViewPresenter<TView> : Presenter<TView>, IModuleViewPresenter<TView>
        where TView : class, IModuleView
    {
        protected ModuleViewPresenter(TView view) : base(view)
        {
            view.Activated += OnActivated;
        }

        public virtual void OnActivated(object sender, EventArgs e)
        {
            View.OnActivated();
        }
    }
}
