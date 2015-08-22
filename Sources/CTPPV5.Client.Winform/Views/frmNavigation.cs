using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CTPPV5.Client.Winform.Events;
using CTPPV5.Client.Winform.Model;
using CTPPV5.Client.Winform.Views.Modules;
using CTPPV5.Infrastructure.Extension;
using WeifenLuo.WinFormsUI.Docking;

namespace CTPPV5.Client.Winform.Views
{
    public partial class frmNavigation : DockContent
    {
        private Dictionary<int, IDocumentModule> moduleMap;
        /// <summary>
        /// 构造函数
        /// </summary>
        public frmNavigation()
        {
            this.moduleMap = new Dictionary<int, IDocumentModule>();
            InitializeComponent();
        }

        public void AddDocumentModule(IDocumentModule module)
        {
            if (!moduleMap.ContainsKey(module.ID))
            {
                var menuItem = module.GetMenuItem(lvwNavigation.Groups);
                menuItem.Tag = module.ID;
                lvwNavigation.Items.Add(menuItem);
                moduleMap[module.ID] = module;
            }
        }

        public void ShowDefaultModule()
        {
            lvwNavigation.Items[0].Selected = true;
            lvwNavigation_Click(lvwNavigation, new EventArgs());
        }

        public void OnModuleDataChanged(object sender, ModelEventArgs args)
        {
            var viewModule = sender as IModule;
            if (viewModule.ID.ToEnum<ModuleType>() == ModuleType.SchoolNav)
            {
                var school = SchoolContext.Get();
                this.lvwNavigation.Groups[0].Header = school.Name;
            }
        }

        /// <summary>
        /// 单击导航菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvwNavigation_Click(object sender, EventArgs e)
        {
            if (lvwNavigation.SelectedItems.Count > 0)
            {
                var moduleId = (int)lvwNavigation.SelectedItems[0].Tag;
                IDocumentModule module = null;
                if (moduleMap.TryGetValue(moduleId, out module)) module.ShowModule();
            }
        }
    }
}
