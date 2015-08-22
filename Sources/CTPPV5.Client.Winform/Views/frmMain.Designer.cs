namespace CTPPV5.Client.Winform.Views
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
            
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.tsmiSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRelogin = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiView = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTool = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiNavigation = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPrevious = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNext = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiClose = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCloseThis = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCloseOthers = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelpBook = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbtnExit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnNavigation = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnBack = new System.Windows.Forms.ToolStripButton();
            this.tsbtnForword = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsddbClose = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolCloseOthers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.tsslUserInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.dpMain = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.ilMain = new System.Windows.Forms.ImageList(this.components);
            this.msMain.SuspendLayout();
            this.tsMain.SuspendLayout();
            this.ssMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMain
            // 
            this.msMain.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSystem,
            this.tsmiView,
            this.tsmiHelp});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(1083, 25);
            this.msMain.TabIndex = 1;
            this.msMain.Text = "menuStrip1";
            // 
            // tsmiSystem
            // 
            this.tsmiSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRelogin,
            this.toolStripSeparator1,
            this.tsmiExit});
            this.tsmiSystem.Name = "tsmiSystem";
            this.tsmiSystem.Size = new System.Drawing.Size(59, 21);
            this.tsmiSystem.Text = "系统(&S)";
            // 
            // tsmiRelogin
            // 
            this.tsmiRelogin.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRelogin.Image")));
            this.tsmiRelogin.Name = "tsmiRelogin";
            this.tsmiRelogin.Size = new System.Drawing.Size(138, 22);
            this.tsmiRelogin.Text = "重新登陆(&L)";
            this.tsmiRelogin.Click += new System.EventHandler(this.tsmiRelogin_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(135, 6);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Image = ((System.Drawing.Image)(resources.GetObject("tsmiExit.Image")));
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(138, 22);
            this.tsmiExit.Text = "退出(&E)";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // tsmiView
            // 
            this.tsmiView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTool,
            this.tsmiStatus,
            this.toolStripSeparator5,
            this.tsmiNavigation,
            this.tsmiPrevious,
            this.tsmiNext,
            this.toolStripSeparator2,
            this.tsmiClose});
            this.tsmiView.Name = "tsmiView";
            this.tsmiView.Size = new System.Drawing.Size(60, 21);
            this.tsmiView.Text = "视图(&V)";
            // 
            // tsmiTool
            // 
            this.tsmiTool.Image = ((System.Drawing.Image)(resources.GetObject("tsmiTool.Image")));
            this.tsmiTool.Name = "tsmiTool";
            this.tsmiTool.Size = new System.Drawing.Size(148, 22);
            this.tsmiTool.Text = "工具栏(&T)";
            this.tsmiTool.Click += new System.EventHandler(this.tsmiTool_Click);
            // 
            // tsmiStatus
            // 
            this.tsmiStatus.Image = ((System.Drawing.Image)(resources.GetObject("tsmiStatus.Image")));
            this.tsmiStatus.Name = "tsmiStatus";
            this.tsmiStatus.Size = new System.Drawing.Size(148, 22);
            this.tsmiStatus.Text = "状态栏(&S)";
            this.tsmiStatus.Click += new System.EventHandler(this.tsmiStatus_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(145, 6);
            // 
            // tsmiNavigation
            // 
            this.tsmiNavigation.Image = ((System.Drawing.Image)(resources.GetObject("tsmiNavigation.Image")));
            this.tsmiNavigation.Name = "tsmiNavigation";
            this.tsmiNavigation.Size = new System.Drawing.Size(148, 22);
            this.tsmiNavigation.Text = "导航菜单(&M)";
            this.tsmiNavigation.Click += new System.EventHandler(this.tsmiNavigation_Click);
            // 
            // tsmiPrevious
            // 
            this.tsmiPrevious.Image = ((System.Drawing.Image)(resources.GetObject("tsmiPrevious.Image")));
            this.tsmiPrevious.Name = "tsmiPrevious";
            this.tsmiPrevious.Size = new System.Drawing.Size(148, 22);
            this.tsmiPrevious.Text = "前一项(&P)";
            this.tsmiPrevious.Click += new System.EventHandler(this.tsmiPrevious_Click);
            // 
            // tsmiNext
            // 
            this.tsmiNext.Image = ((System.Drawing.Image)(resources.GetObject("tsmiNext.Image")));
            this.tsmiNext.Name = "tsmiNext";
            this.tsmiNext.Size = new System.Drawing.Size(148, 22);
            this.tsmiNext.Text = "后一项(&N)";
            this.tsmiNext.Click += new System.EventHandler(this.tsmiNext_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(145, 6);
            // 
            // tsmiClose
            // 
            this.tsmiClose.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCloseThis,
            this.tsmiCloseOthers,
            this.tsmiCloseAll});
            this.tsmiClose.Image = ((System.Drawing.Image)(resources.GetObject("tsmiClose.Image")));
            this.tsmiClose.Name = "tsmiClose";
            this.tsmiClose.Size = new System.Drawing.Size(148, 22);
            this.tsmiClose.Text = "关闭(&C)";
            // 
            // tsmiCloseThis
            // 
            this.tsmiCloseThis.Name = "tsmiCloseThis";
            this.tsmiCloseThis.Size = new System.Drawing.Size(142, 22);
            this.tsmiCloseThis.Text = "关闭(&C)";
            this.tsmiCloseThis.Click += new System.EventHandler(this.tsmiCloseThis_Click);
            // 
            // tsmiCloseOthers
            // 
            this.tsmiCloseOthers.Name = "tsmiCloseOthers";
            this.tsmiCloseOthers.Size = new System.Drawing.Size(142, 22);
            this.tsmiCloseOthers.Text = "关闭其他(&O)";
            this.tsmiCloseOthers.Click += new System.EventHandler(this.tsmiCloseOthers_Click);
            // 
            // tsmiCloseAll
            // 
            this.tsmiCloseAll.Name = "tsmiCloseAll";
            this.tsmiCloseAll.Size = new System.Drawing.Size(142, 22);
            this.tsmiCloseAll.Text = "关闭全部(&A)";
            this.tsmiCloseAll.Click += new System.EventHandler(this.tsmiCloseAll_Click);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiHelpBook,
            this.tsmiAbout});
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(61, 21);
            this.tsmiHelp.Text = "帮助(&H)";
            // 
            // tsmiHelpBook
            // 
            this.tsmiHelpBook.Image = ((System.Drawing.Image)(resources.GetObject("tsmiHelpBook.Image")));
            this.tsmiHelpBook.Name = "tsmiHelpBook";
            this.tsmiHelpBook.Size = new System.Drawing.Size(117, 22);
            this.tsmiHelpBook.Text = "帮助(&H)";
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(117, 22);
            this.tsmiAbout.Text = "关于(&A)";
            // 
            // tsMain
            // 
            this.tsMain.AutoSize = false;
            this.tsMain.Font = new System.Drawing.Font("SimSun", 9F);
            this.tsMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnExit,
            this.toolStripSeparator4,
            this.tsbtnNavigation,
            this.toolStripSeparator3,
            this.tsbtnBack,
            this.tsbtnForword,
            this.toolStripSeparator6,
            this.tsddbClose});
            this.tsMain.Location = new System.Drawing.Point(0, 25);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(1083, 32);
            this.tsMain.TabIndex = 3;
            this.tsMain.Text = "toolStrip1";
            // 
            // tsbtnExit
            // 
            this.tsbtnExit.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnExit.Image")));
            this.tsbtnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnExit.Name = "tsbtnExit";
            this.tsbtnExit.Size = new System.Drawing.Size(57, 29);
            this.tsbtnExit.Text = "退出";
            this.tsbtnExit.ToolTipText = "退出";
            this.tsbtnExit.Click += new System.EventHandler(this.tsbtnExit_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 32);
            // 
            // tsbtnNavigation
            // 
            this.tsbtnNavigation.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnNavigation.Image")));
            this.tsbtnNavigation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnNavigation.Name = "tsbtnNavigation";
            this.tsbtnNavigation.Size = new System.Drawing.Size(81, 29);
            this.tsbtnNavigation.Text = "导航菜单";
            this.tsbtnNavigation.Click += new System.EventHandler(this.tsbtnNavigation_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 32);
            // 
            // tsbtnBack
            // 
            this.tsbtnBack.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnBack.Image")));
            this.tsbtnBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnBack.Name = "tsbtnBack";
            this.tsbtnBack.Size = new System.Drawing.Size(57, 29);
            this.tsbtnBack.Text = "前项";
            this.tsbtnBack.Click += new System.EventHandler(this.tsbtnBack_Click);
            // 
            // tsbtnForword
            // 
            this.tsbtnForword.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnForword.Image")));
            this.tsbtnForword.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnForword.Name = "tsbtnForword";
            this.tsbtnForword.Size = new System.Drawing.Size(57, 29);
            this.tsbtnForword.Text = "后项";
            this.tsbtnForword.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.tsbtnForword.Click += new System.EventHandler(this.tsbtnForword_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 32);
            // 
            // tsddbClose
            // 
            this.tsddbClose.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolClose,
            this.toolCloseOthers,
            this.toolCloseAll});
            this.tsddbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsddbClose.Image")));
            this.tsddbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbClose.Name = "tsddbClose";
            this.tsddbClose.Size = new System.Drawing.Size(66, 29);
            this.tsddbClose.Text = "关闭";
            // 
            // toolClose
            // 
            this.toolClose.Name = "toolClose";
            this.toolClose.Size = new System.Drawing.Size(136, 22);
            this.toolClose.Text = "关闭(&C)";
            this.toolClose.Click += new System.EventHandler(this.toolClose_Click);
            // 
            // toolCloseOthers
            // 
            this.toolCloseOthers.Name = "toolCloseOthers";
            this.toolCloseOthers.Size = new System.Drawing.Size(136, 22);
            this.toolCloseOthers.Text = "关闭其他(&O)";
            this.toolCloseOthers.Click += new System.EventHandler(this.toolCloseOthers_Click);
            // 
            // toolCloseAll
            // 
            this.toolCloseAll.Name = "toolCloseAll";
            this.toolCloseAll.Size = new System.Drawing.Size(136, 22);
            this.toolCloseAll.Text = "关闭全部(&A)";
            this.toolCloseAll.Click += new System.EventHandler(this.toolCloseAll_Click);
            // 
            // ssMain
            // 
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslUserInfo});
            this.ssMain.Location = new System.Drawing.Point(0, 631);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(1083, 22);
            this.ssMain.TabIndex = 5;
            this.ssMain.Text = "statusStrip1";
            // 
            // tsslUserInfo
            // 
            this.tsslUserInfo.Image = ((System.Drawing.Image)(resources.GetObject("tsslUserInfo.Image")));
            this.tsslUserInfo.Name = "tsslUserInfo";
            this.tsslUserInfo.Size = new System.Drawing.Size(72, 17);
            this.tsslUserInfo.Text = "用户信息";
            this.tsslUserInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dpMain
            // 
            this.dpMain.ActiveAutoHideContent = null;
            this.dpMain.AllowDrop = true;
            this.dpMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.dpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dpMain.DockLeftPortion = 180D;
            this.dpMain.DockRightPortion = 200D;
            this.dpMain.Location = new System.Drawing.Point(0, 57);
            this.dpMain.Name = "dpMain";
            this.dpMain.ShowDocumentIcon = true;
            this.dpMain.Size = new System.Drawing.Size(1083, 574);
            this.dpMain.TabIndex = 7;
            this.dpMain.ActiveDocumentChanged += new System.EventHandler(this.dpMain_ActiveDocumentChanged);
            // 
            // ilMain
            // 
            this.ilMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilMain.ImageStream")));
            this.ilMain.TransparentColor = System.Drawing.Color.Transparent;
            this.ilMain.Images.SetKeyName(0, "check");
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1083, 653);
            this.Controls.Add(this.dpMain);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.tsMain);
            this.Controls.Add(this.msMain);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "创智晨检管理系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem tsmiSystem;
        private System.Windows.Forms.ToolStripMenuItem tsmiRelogin;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ToolStripMenuItem tsmiView;
        private System.Windows.Forms.ToolStripMenuItem tsmiTool;
        private System.Windows.Forms.ToolStripMenuItem tsmiStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tsmiNavigation;
        private System.Windows.Forms.ToolStripMenuItem tsmiPrevious;
        private System.Windows.Forms.ToolStripMenuItem tsmiNext;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiClose;
        private System.Windows.Forms.ToolStripMenuItem tsmiCloseThis;
        private System.Windows.Forms.ToolStripMenuItem tsmiCloseOthers;
        private System.Windows.Forms.ToolStripMenuItem tsmiCloseAll;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelpBook;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton tsbtnExit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbtnNavigation;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbtnBack;
        private System.Windows.Forms.ToolStripButton tsbtnForword;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripDropDownButton tsddbClose;
        private System.Windows.Forms.ToolStripMenuItem toolClose;
        private System.Windows.Forms.ToolStripMenuItem toolCloseOthers;
        private System.Windows.Forms.ToolStripMenuItem toolCloseAll;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripStatusLabel tsslUserInfo;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dpMain;
        private System.Windows.Forms.ImageList ilMain;
    }
}