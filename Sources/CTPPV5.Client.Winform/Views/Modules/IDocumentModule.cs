using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CTPPV5.Client.Winform.Views.Modules
{
    public interface IDocumentModule : IModule
    {
        void ShowModule();
        void OnActivate();
        ListViewItem GetMenuItem(ListViewGroupCollection groups);
    }
}
