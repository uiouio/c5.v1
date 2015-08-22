using System.ComponentModel;
using System.Windows.Forms;
using WinFormsMvp.Binder;

namespace WinFormsMvp.Forms
{
    public class MvpForm : Form, IView<object>
    {
        private readonly PresenterBinder presenterBinder = new PresenterBinder();

        public MvpForm()
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
