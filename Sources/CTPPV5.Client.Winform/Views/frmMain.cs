using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsMvp;
using WeifenLuo.WinFormsUI.Docking;
using WinFormsMvp.Forms;
using CTPPV5.Client.Winform.Presenters;
using CTPPV5.Client.Winform.Views.Modules;
using CTPPV5.Client.Winform.Model;
using CTPPV5.Client.Winform.Events;
using CTPPV5.Infrastructure.Extension;

namespace CTPPV5.Client.Winform.Views
{
    public interface IModuleContainerView : IView<object>
    {
        DockPanel ModuleContainer { get; }
        void OnLoad();
        void OnModuleAdd(IModule module);
    }

    [PresenterBinding(typeof(IModuleContainerPresenter))]
    public partial class frmMain : MvpForm, IModuleContainerView
    {
        private bool m_ReLogin = false;
        private bool m_HideToolStrip = false;
        private bool m_HideStatusStrip = false;
        private frmNavigation m_NavigationForm = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
            m_NavigationForm = new frmNavigation();
        }

        public DockPanel ModuleContainer { get { return dpMain; } }

        public void OnLoad()
        {
            var user = UserContext.Get();
            m_NavigationForm.Show(dpMain, DockState.DockLeft);
            m_NavigationForm.ShowDefaultModule();
            RefreshControls();
            tsslUserInfo.Text = String.Format("登录用户:{0}   {1}    登录时间:{2}", user.Name, user.Group, user.LastLoginTime.ToString("MM月dd日 HH时mm分"));
        }

        public void OnModuleAdd(IModule module)
        {
            var navModule = module as IToolStripModule;
            if (navModule != null) tsMain.Items.Add(navModule.NavButton);
            var rightDockModule = module as IRightDockPanelModule;
            if (rightDockModule != null)
            {
                rightDockModule.ModuleDataChanged += m_NavigationForm.OnModuleDataChanged;
                rightDockModule.ModuleDataChanged += this.OnModuleDataChanged;
                rightDockModule.ShowModule();
            }
            var docModule = module as IDocumentModule;
            if (docModule != null)
            {
                m_NavigationForm.AddDocumentModule(docModule);
            }
        }

        private void OnModuleDataChanged(object sender, ModelEventArgs args)
        {
            var viewModule = sender as IModule;
            if (viewModule.ID.ToEnum<ModuleType>() == ModuleType.SchoolNav)
            {
                CloseAllDocks();
                m_NavigationForm.ShowDefaultModule();
            }
        }

        /// <summary>
        /// 重新登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiRelogin_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要退出重新登录系统吗？", "重新登录", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                m_ReLogin = true;
                Application.Restart();
            }
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 隐藏/显示工具栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiTool_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_HideToolStrip)
                {
                    tsMain.Show();
                    tsmiTool.Image = ilMain.Images["check"];
                }
                else
                {
                    tsMain.Hide();
                    tsmiTool.Image = null;
                }
                m_HideToolStrip = !m_HideToolStrip;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "隐藏/显示工具栏发生异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 隐藏/显示状态栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_HideStatusStrip)
                {
                    ssMain.Show();
                    tsmiStatus.Image = ilMain.Images["check"];
                }
                else
                {
                    ssMain.Hide();
                    tsmiStatus.Image = null;
                }
                m_HideStatusStrip = !m_HideStatusStrip;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "隐藏/显示状态栏发生异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示/隐藏导航菜单面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiNavigation_Click(object sender, EventArgs e)
        {
            OptNavigationPanel();
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 显示/隐藏导航菜单面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnNavigation_Click(object sender, EventArgs e)
        {
            OptNavigationPanel();
        }

        /// <summary>
        /// 退出前提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_ReLogin)
            {
                if (MessageBox.Show("您确定要退出系统吗？", "退出系统", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// 激活DockDocument刷新控件状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dpMain_ActiveDocumentChanged(object sender, EventArgs e)
        {
            RefreshControls();
        }

        /// <summary>
        /// 关闭所有的Dock窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiCloseAll_Click(object sender, EventArgs e)
        {
            CloseAllDocks();
        }

        /// <summary>
        /// 关闭其他Dock窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiCloseOthers_Click(object sender, EventArgs e)
        {
            CloseOtherDocks();
        }

        /// <summary>
        /// 关闭当前Dock窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiCloseThis_Click(object sender, EventArgs e)
        {
            CloseCurDock();
        }

        /// <summary>
        /// 关闭当前Dock窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolClose_Click(object sender, EventArgs e)
        {
            CloseCurDock();
        }

        /// <summary>
        /// 关闭其他Dock窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolCloseOthers_Click(object sender, EventArgs e)
        {
            CloseOtherDocks();
        }

        /// <summary>
        /// 关闭所有的Dock窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolCloseAll_Click(object sender, EventArgs e)
        {
            CloseAllDocks();
        }

        /// <summary>
        /// 激活左侧Dock窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiPrevious_Click(object sender, EventArgs e)
        {
            GoBack();
        }

        /// <summary>
        /// 激活右侧Dock窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiNext_Click(object sender, EventArgs e)
        {
            GoFoward();
        }

        /// <summary>
        /// 激活左侧Dock窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnBack_Click(object sender, EventArgs e)
        {
            GoBack();
        }

        /// <summary>
        /// 激活右侧Dock窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnForword_Click(object sender, EventArgs e)
        {
            GoFoward();
        }

        #region Private Method
        /// <summary>
        /// 显示/隐藏导航菜单面板
        /// </summary>
        private void OptNavigationPanel()
        {
            if (m_NavigationForm == null || m_NavigationForm.IsDisposed)
            {
                m_NavigationForm = new frmNavigation();
            }

            if (m_NavigationForm.DockPanel == null)
            {
                m_NavigationForm.Show(dpMain, DockState.DockLeft);
            }
            else
            {
                if (m_NavigationForm.IsHidden)
                {
                    m_NavigationForm.Show(dpMain, DockState.DockLeft);
                }
                else if (m_NavigationForm.DockState == DockState.DockLeftAutoHide)
                {
                    m_NavigationForm.Show(dpMain, DockState.DockLeft);
                }
                else
                {
                    m_NavigationForm.DockPanel = null;
                }
            }
        }

        /// <summary>
        /// 刷新控件状态
        /// </summary>
        private void RefreshControls()
        {
            if (dpMain.DocumentsCount == 0)
            {
                tsmiClose.Enabled = false;
                tsmiCloseThis.Enabled = false;
                tsmiCloseOthers.Enabled = false;
                tsmiCloseAll.Enabled = false;

                tsddbClose.Enabled = false;
                toolClose.Enabled = false;
                toolCloseOthers.Enabled = false;
                toolCloseAll.Enabled = false;

                tsmiPrevious.Enabled = false;
                tsmiNext.Enabled = false;

                tsbtnForword.Enabled = false;
                tsbtnBack.Enabled = false;
            }
            else
            {
                if (dpMain.DocumentsCount == 1)
                {
                    tsmiClose.Enabled = true;
                    tsmiCloseThis.Enabled = true;
                    tsmiCloseOthers.Enabled = false;
                    tsmiCloseAll.Enabled = true;

                    tsddbClose.Enabled = true;
                    toolClose.Enabled = true;
                    toolCloseOthers.Enabled = false;
                    toolCloseAll.Enabled = true;

                    tsmiPrevious.Enabled = false;
                    tsmiNext.Enabled = false;

                    tsbtnForword.Enabled = false;
                    tsbtnBack.Enabled = false;
                }
                else
                {
                    tsmiClose.Enabled = true;
                    tsmiCloseThis.Enabled = true;
                    tsmiCloseOthers.Enabled = true;
                    tsmiCloseAll.Enabled = true;

                    tsddbClose.Enabled = true;
                    toolClose.Enabled = true;
                    toolCloseOthers.Enabled = true;
                    toolCloseAll.Enabled = true;

                    tsmiPrevious.Enabled = true;
                    tsmiNext.Enabled = true;

                    tsbtnForword.Enabled = true;
                    tsbtnBack.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 当切换学校时初始化Dock窗口
        /// </summary>
        private void RefreshDocks()
        {
            CloseAllDocks();
            m_NavigationForm.ShowDefaultModule();
        }

        /// <summary>
        /// 获取所有的Dock窗口
        /// </summary>
        /// <returns></returns>
        private List<DockContent> GetAllDocks()
        {
            List<DockContent> result = new List<DockContent>();
            foreach (DockContent dc in this.dpMain.Contents)
            {
                if (dc.Tag != null)
                {
                    result.Add(dc);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取当前的Dock窗口
        /// </summary>
        /// <returns></returns>
        private DockContent GetCurDock()
        {
            DockContent result = null;
            foreach (DockContent dc in this.dpMain.Contents)
            {
                if (dc.Tag != null && dc.IsActivated)
                {
                    result = dc;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取所有其他的Dock窗口
        /// </summary>
        /// <returns></returns>
        private List<DockContent> GetOtherDocks()
        {
            List<DockContent> result = new List<DockContent>();
            foreach (DockContent dc in this.dpMain.Contents)
            {
                if (dc.Tag != null && !dc.IsActivated)
                {
                    result.Add(dc);
                }
            }
            return result;
        }

        /// <summary>
        /// 关闭所有的Dock窗口
        /// </summary>
        private void CloseAllDocks()
        {
            foreach (var dc in this.GetAllDocks())
            {
                dc.Hide();
            }
        }

        /// <summary>
        /// 关闭当前Dock窗口
        /// </summary>
        private void CloseCurDock()
        {
            DockContent dc = this.GetCurDock();
            if (dc != null)
            {
                dc.Hide();
            }
        }

        /// <summary>
        /// 关闭其他的Dock窗口
        /// </summary>
        private void CloseOtherDocks()
        {
            foreach (var dc in this.GetOtherDocks())
            {
                dc.Hide();
            }
        }

        /// <summary>
        /// 前一个Dock窗口
        /// </summary>
        private void GoBack()
        {
            if (this.dpMain.ActiveContent != null)
            {
                int curIndex = this.dpMain.Contents.IndexOf(this.dpMain.ActiveContent);
                for (var idx = curIndex - 1; idx >= 0; idx++)
                {
                    var previous = (DockContent)dpMain.Contents[idx];
                    if (!previous.IsHidden)
                    {
                        previous.Activate();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 后一个Dock窗口
        /// </summary>
        private void GoFoward()
        {
            if (this.dpMain.ActiveContent != null)
            {
                int curIndex = this.dpMain.Contents.IndexOf(this.dpMain.ActiveContent);
                for (var idx = curIndex + 1; idx < dpMain.Contents.Count; idx++)
                {
                    var next = (DockContent)dpMain.Contents[idx];
                    if (!next.IsHidden)
                    {
                        next.Activate();
                        break;
                    }
                }
            }
        }
        #endregion

    }
}
