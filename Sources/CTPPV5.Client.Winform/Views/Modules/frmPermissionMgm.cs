using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using CTPPV5.Infrastructure.Extension;
using WinFormsMvp;
using CTPPV5.Client.Winform.Presenters;
using CTPPV5.Client.Winform.Model;

namespace CTPPV5.Client.Winform.Views.Modules
{
    public interface IPermissionMgmView : IModuleView
    {
    }

    [PresenterBinding(typeof(IPermissionMgmPresenter))]
    public partial class frmPermissionMgm : AbstractDocumentModule, IPermissionMgmView
    {
        public frmPermissionMgm(DockPanel parent) :base(parent)
        {
            InitializeComponent();
        }

        public override int ID { get { return ModuleType.PermissionMgm.ToInt32(); } }

        public override ListViewItem GetMenuItem(ListViewGroupCollection groups)
        {
            var listViewItem8 = new System.Windows.Forms.ListViewItem("权限管理", 2);
            listViewItem8.Group = groups.Cast<ListViewGroup>()
                .Where(l => l.Name.Equals("lvgManage")).FirstOrDefault();
            listViewItem8.ToolTipText = "权限管理";
            return listViewItem8;
        }

        public void OnActivated()
        {
            MessageBox.Show(string.Format("activate:{0}, moduleId:{1}, schoolId:{2}", this.GetType().Name, this.ID, SchoolContext.Get().ID));
        }

        protected override DockContent Content { get { return this; } }
    }
}
