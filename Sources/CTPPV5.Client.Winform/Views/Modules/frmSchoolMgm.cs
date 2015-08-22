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
    public interface ISchoolMgmView : IModuleView
    {
    }

    [PresenterBinding(typeof(ISchoolMgmPresenter))]
    public partial class frmSchoolMgm : AbstractDocumentModule, ISchoolMgmView
    {
        public frmSchoolMgm(DockPanel parent) :base(parent)
        {
            InitializeComponent();
        }

        public override int ID { get { return ModuleType.SchoolMgm.ToInt32(); } }

        public override ListViewItem GetMenuItem(ListViewGroupCollection groups)
        {
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("学校管理", 0);
            listViewItem6.Group = groups.Cast<ListViewGroup>()
                .Where(l => l.Name.Equals("lvgSchool")).FirstOrDefault();
            listViewItem6.ToolTipText = "学校管理";
            return listViewItem6;
        }

        public void OnActivated()
        {
            MessageBox.Show(string.Format("activate:{0}, moduleId:{1}, schoolId:{2}", this.GetType().Name, this.ID, SchoolContext.Get().ID));
        }

        protected override DockContent Content { get { return this; } }
    }
}
