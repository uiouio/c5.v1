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
using CTPPV5.Client.Winform.Events;
using CTPPV5.Models;
using WinFormsMvp;
using CTPPV5.Client.Winform.Presenters;

namespace CTPPV5.Client.Winform.Views.Modules
{
    public interface ISchoolNavView : IView<object>
    {
        void OnLoad();
        void OnAddSchoolsGroupByProvince(Province province);
        void OnAddFinished();

        event EventHandler<ModelEventArgs> SchoolFocused;
    }

    [PresenterBinding(typeof(ISchoolNavPresenter))]
    public partial class frmSchoolNav : MvpDock, IToolStripModule, IRightDockPanelModule, ISchoolNavView
    {
        private DockPanel parent;
        
        public frmSchoolNav(DockPanel parent)
        {
            this.parent = parent;
            this.InitializeComponent();
        }

        public int ID { get { return ModuleType.SchoolNav.ToInt32(); } }

        public event EventHandler<ModelEventArgs> ModuleDataChanged;

        public ToolStripButton NavButton
        {
            get
            {
                var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
                var tsbtnSchoolNav = new System.Windows.Forms.ToolStripButton();
                tsbtnSchoolNav.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSchoolNav.Image")));
                tsbtnSchoolNav.ImageTransparentColor = System.Drawing.Color.Magenta;
                tsbtnSchoolNav.Name = "tsbtnSchoolNav";
                tsbtnSchoolNav.Size = new System.Drawing.Size(93, 29);
                tsbtnSchoolNav.Text = "学校导航栏";
                tsbtnSchoolNav.Click += new System.EventHandler(this.tsbtnSchoolNav_Click);
                return tsbtnSchoolNav;
            }
        }

        public void OnLoad()
        {
            tvSchoolNav.Nodes.Clear();
            TreeNode rootNode = new TreeNode("所有学校", 0, 0);
            tvSchoolNav.Nodes.Add(rootNode);
        }

        public void OnAddSchoolsGroupByProvince(Province province)
        {
            var rootNode = tvSchoolNav.Nodes[0];
            var provinceNode = new TreeNode(province.Name, 1, 1);
            foreach (var area in province.Areas)
            {
                var areaNode = new TreeNode(area.Name, 1, 1);
                foreach (var school in area.Schools)
                {
                    areaNode.Nodes.Add(new TreeNode(school.Name, 2, 2) { Tag = school, Name = "leaf" });
                }
                provinceNode.Nodes.Add(areaNode);
            }
            rootNode.Nodes.Add(provinceNode);
        }

        public void OnAddFinished()
        {
            tvSchoolNav.SelectedNode = tvSchoolNav.Nodes[0];
            tvSchoolNav.ExpandAll();
            ChooseTheFirstNode();
        }

        public void ShowModule()
        {
            this.Show(parent, DockState.DockRight);
        }

        public event EventHandler<ModelEventArgs> SchoolFocused;

        /// <summary>
        /// 选中学校修改导航菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvSchoolNav_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                if (SchoolFocused != null) SchoolFocused(this, new ModelEventArgs(e.Node.Tag));
                if (ModuleDataChanged != null) ModuleDataChanged(this, ModelEventArgs.Instance);
            }
        }

        /// <summary>
        /// 默认选中第一个学校
        /// </summary>
        private void ChooseTheFirstNode()
        {
            var leaf = tvSchoolNav.Nodes.Find("leaf", true).FirstOrDefault();
            if (leaf != null)
            {
                if (SchoolFocused != null) SchoolFocused(this, new ModelEventArgs(leaf.Tag));
                if (ModuleDataChanged != null) ModuleDataChanged(this, ModelEventArgs.Instance);
            }
        }

        /// <summary>
        /// 显示/隐藏学校导航栏面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnSchoolNav_Click(object sender, EventArgs e)
        {
            if (!this.IsDisposed)
            {
                if (this.IsHidden) this.Show();
                else this.Hide();
            }
        }
    }
}
