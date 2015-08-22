using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMvp;

namespace CTPPV5.Client.Winform.Views.Modules
{
    public interface IModuleView : IView<object>
    {
        event EventHandler Activated;
        void OnActivated();
    }
}
