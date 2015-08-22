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
    public interface IUpdatePwdView : IModuleView
    {
    }

    [PresenterBinding(typeof(IUpdatePwdPresenter))]
    public partial class frmUpdatePwd : AbstractDocumentModule, IUpdatePwdView
    {
        public frmUpdatePwd(DockPanel parent) : base(parent)
        {
            InitializeComponent();
        }

        public override int ID { get { return ModuleType.UpdatePwd.ToInt32(); } }

        public override ListViewItem GetMenuItem(ListViewGroupCollection groups)
        {
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("修改密码", 3);
            listViewItem9.Group = groups.Cast<ListViewGroup>()
                .Where(l => l.Name.Equals("lvgManage")).FirstOrDefault();
            listViewItem9.ToolTipText = "修改密码";
            return listViewItem9;
        }

        public void OnActivated()
        {
            MessageBox.Show(string.Format("activate:{0}, moduleId:{1}, schoolId:{2}", this.GetType().Name, this.ID, SchoolContext.Get().ID));
        }

        protected override DockContent Content { get { return this; } }
    }
}
