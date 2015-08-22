using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;
using WinFormsMvp;
using WinFormsMvp.Binder;

namespace CTPPV5.Client.Winform.Views
{
    public class MvpDock : DockContent, IView<object>
    {
        private readonly PresenterBinder presenterBinder = new PresenterBinder();

        public MvpDock()
        {
            ThrowExceptionIfNoPresenterBound = true;

            presenterBinder.PerformBinding(this);
        }

        #region Implementation of IView<TModel>

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Model { get; set; }

        #endregion

        #region Implementation of IView

        public bool ThrowExceptionIfNoPresenterBound { get; private set; }

        #endregion
    }
}
