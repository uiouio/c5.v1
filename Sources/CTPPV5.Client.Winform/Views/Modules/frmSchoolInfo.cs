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
    public interface ISchoolInfoView : IModuleView
    {
    }

    [PresenterBinding(typeof(ISchoolInfoPresenter))]
    public partial class frmSchoolInfo : AbstractDocumentModule, ISchoolInfoView
    {
        public frmSchoolInfo(DockPanel parent) :base(parent)
        {
            InitializeComponent();
        }

        public override int ID { get { return ModuleType.SchoolInfo.ToInt32(); } }
        public override ListViewItem GetMenuItem(ListViewGroupCollection groups)
        {
            var listViewItem1 = new System.Windows.Forms.ListViewItem("基本信息", 4);
            listViewItem1.Group = groups.Cast<ListViewGroup>()
                .Where(l => l.Name.Equals("lvgSchool")).FirstOrDefault();
            listViewItem1.ToolTipText = "基本信息";
            return listViewItem1;
        }

        public void OnActivated()
        {
            MessageBox.Show(string.Format("activate:{0}, moduleId:{1}, schoolId:{2}", this.GetType().Name, this.ID, SchoolContext.Get().ID));
        }

        protected override DockContent Content { get { return this; } }
    }
}
