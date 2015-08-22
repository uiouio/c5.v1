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
    public interface ICardMachineView : IModuleView
    {

    }

    [PresenterBinding(typeof(ICardMachinePresenter))]
    public partial class frmCardMachine : AbstractDocumentModule, ICardMachineView
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public frmCardMachine(DockPanel parent) :base(parent)
        {
            InitializeComponent();
        }

        public override int ID { get { return ModuleType.CardMachine.ToInt32(); } }

        public override ListViewItem GetMenuItem(ListViewGroupCollection groups)
        {
            var listViewItem5 = new System.Windows.Forms.ListViewItem("卡片机管理", 8);
            listViewItem5.Group = groups.Cast<ListViewGroup>()
                .Where(l => l.Name.Equals("lvgManage")).FirstOrDefault();
            listViewItem5.ToolTipText = "卡片机管理";
            return listViewItem5;
        }

        public void OnActivated()
        {
            MessageBox.Show(string.Format("activate:{0}, moduleId:{1}, schoolId:{2}", this.GetType().Name, this.ID, SchoolContext.Get().ID));
        }

        protected override DockContent Content { get { return this; } }
    }
}
