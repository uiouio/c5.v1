using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CTPPV5.Client.Winform.Events;
using WeifenLuo.WinFormsUI.Docking;
using WinFormsMvp;

namespace CTPPV5.Client.Winform.Views.Modules
{
    public interface IModule
    {
        int ID { get; }
        event EventHandler<ModelEventArgs> ModuleDataChanged;
    }
}
