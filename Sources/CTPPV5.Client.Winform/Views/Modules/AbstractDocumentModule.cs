using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CTPPV5.Client.Winform.Events;
using WeifenLuo.WinFormsUI.Docking;
using System.ComponentModel;

namespace CTPPV5.Client.Winform.Views.Modules
{
    public class AbstractDocumentModule : MvpDock, IDocumentModule
    {
        private DockPanel parent;
        public AbstractDocumentModule() { }
        public AbstractDocumentModule(DockPanel parent)
        {
            this.parent = parent;
        }

        public event EventHandler<ModelEventArgs> ModuleDataChanged;

        /// <summary>
        /// 显示导航菜单对应的功能模块
        /// </summary>
        public void ShowModule()
        {
            DockContent dc = GetAlreadyAdded();
            if (dc != null)
            {
                if (dc.IsHidden) dc.Activate();
            }
            else 
            {
                Content.Tag = ID;
                Content.Show(parent, DockState.Document);
            }
        }
        public virtual int ID { get { throw new NotImplementedException(); } }
        public virtual void OnActivate() { throw new NotImplementedException(); }
        public virtual ListViewItem GetMenuItem(ListViewGroupCollection groups) { throw new NotImplementedException(); }
        protected virtual DockContent Content { get { throw new NotImplementedException(); } }

        /// <summary>
        /// 查找已打开的DockContent
        /// </summary>
        private DockContent GetAlreadyAdded()
        {
            DockContent result = null;
            foreach (DockContent dc in parent.Contents)
            {
                if (dc.Tag != null && (int)dc.Tag == ID)
                {
                    result = dc;
                    break;
                }
            }
            return result;
        }
    }
}
