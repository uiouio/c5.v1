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
    public interface ITeacherMgmView : IModuleView
    {
    }

    [PresenterBinding(typeof(ITeacherMgmPresenter))]
    public partial class frmTeacherMgm : AbstractDocumentModule, ITeacherMgmView
    {
        public frmTeacherMgm(DockPanel parent) :base(parent)
        {
            InitializeComponent();
        }

        public override int ID { get { return ModuleType.TeacherMgm.ToInt32(); } }

        public override ListViewItem GetMenuItem(ListViewGroupCollection groups)
        {
            var listViewItem4 = new System.Windows.Forms.ListViewItem("老师管理", 7);
            listViewItem4.Group = groups.Cast<ListViewGroup>()
                .Where(l => l.Name.Equals("lvgSchool")).FirstOrDefault();
            listViewItem4.ToolTipText = "老师管理";
            return listViewItem4;
        }

        public void OnActivated()
        {
            MessageBox.Show(string.Format("activate:{0}, moduleId:{1}, schoolId:{2}", this.GetType().Name, this.ID, SchoolContext.Get().ID));
        }

        protected override DockContent Content { get { return this; } }
    }
}
