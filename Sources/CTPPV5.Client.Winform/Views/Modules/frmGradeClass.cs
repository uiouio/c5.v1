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
    public interface IGradeClassView : IModuleView
    {

    }

    [PresenterBinding(typeof(IGradeClassPresenter))]
    public partial class frmGradeClass : AbstractDocumentModule, IGradeClassView
    {
        public frmGradeClass(DockPanel parent) :base(parent)
        {
            InitializeComponent();
        }

        public override int ID { get { return ModuleType.GradeClass.ToInt32(); } }
        public override ListViewItem GetMenuItem(ListViewGroupCollection groups)
        {
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("年班管理", 5);
            listViewItem2.Group = groups.Cast<ListViewGroup>()
                .Where(l => l.Name.Equals("lvgSchool")).FirstOrDefault();
            listViewItem2.ToolTipText = "年班管理";
            return listViewItem2;
        }

        public void OnActivated()
        {
            MessageBox.Show(string.Format("activate:{0}, moduleId:{1}, schoolId:{2}", this.GetType().Name, this.ID, SchoolContext.Get().ID));
        }

        protected override DockContent Content { get { return this; } }
    }
}
