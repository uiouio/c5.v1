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
    public interface IStudentMgmView : IModuleView
    {
    }

    [PresenterBinding(typeof(IStudentMgmPresenter))]
    public partial class frmStudentMgm : AbstractDocumentModule, IStudentMgmView
    {
        public frmStudentMgm(DockPanel parent) :base(parent)
        {
            InitializeComponent();
        }

        public override int ID { get { return ModuleType.StudentMgm.ToInt32(); } }

        public override ListViewItem GetMenuItem(ListViewGroupCollection groups)
        {
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("学生管理", 6);
            listViewItem3.Group = groups.Cast<ListViewGroup>()
                .Where(l => l.Name.Equals("lvgSchool")).FirstOrDefault();
            listViewItem3.ToolTipText = "学生管理";
            return listViewItem3;
        }

        public void OnActivated()
        {
            MessageBox.Show(string.Format("activate:{0}, moduleId:{1}, schoolId:{2}", this.GetType().Name, this.ID, SchoolContext.Get().ID));
        }

        protected override DockContent Content { get { return this; } }
    }
}
