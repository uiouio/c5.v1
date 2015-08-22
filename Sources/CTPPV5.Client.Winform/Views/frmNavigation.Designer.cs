namespace CTPPV5.Client.Winform.Views
{
    partial class frmNavigation
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("学校管理", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("管理中心", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNavigation));
            this.lvwNavigation = new System.Windows.Forms.ListView();
            this.ilNavigation = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // lvwNavigation
            // 
            this.lvwNavigation.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvwNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup1.Header = "学校管理";
            listViewGroup1.Name = "lvgSchool";
            listViewGroup2.Header = "管理中心";
            listViewGroup2.Name = "lvgManage";
            this.lvwNavigation.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.lvwNavigation.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwNavigation.LargeImageList = this.ilNavigation;
            this.lvwNavigation.Location = new System.Drawing.Point(0, 0);
            this.lvwNavigation.MultiSelect = false;
            this.lvwNavigation.Name = "lvwNavigation";
            this.lvwNavigation.Scrollable = false;
            this.lvwNavigation.Size = new System.Drawing.Size(204, 677);
            this.lvwNavigation.SmallImageList = this.ilNavigation;
            this.lvwNavigation.StateImageList = this.ilNavigation;
            this.lvwNavigation.TabIndex = 0;
            this.lvwNavigation.UseCompatibleStateImageBehavior = false;
            this.lvwNavigation.View = System.Windows.Forms.View.Tile;
            this.lvwNavigation.Click += new System.EventHandler(this.lvwNavigation_Click);
            // 
            // ilNavigation
            // 
            this.ilNavigation.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilNavigation.ImageStream")));
            this.ilNavigation.TransparentColor = System.Drawing.Color.Transparent;
            this.ilNavigation.Images.SetKeyName(0, "users.ico");
            this.ilNavigation.Images.SetKeyName(1, "user.ico");
            this.ilNavigation.Images.SetKeyName(2, "preferences_system.ico");
            this.ilNavigation.Images.SetKeyName(3, "decrypted.ico");
            this.ilNavigation.Images.SetKeyName(4, "info.ico");
            this.ilNavigation.Images.SetKeyName(5, "document_folder_blue.ico");
            this.ilNavigation.Images.SetKeyName(6, "user_male_white_blue_grey.ico");
            this.ilNavigation.Images.SetKeyName(7, "teacher.png");
            this.ilNavigation.Images.SetKeyName(8, "disk_silver_time_machine.ico");
            // 
            // frmNavigation
            // 
            this.AllowEndUserDocking = false;
            this.AutoHidePortion = 0.16D;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 677);
            this.Controls.Add(this.lvwNavigation);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft;
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmNavigation";
            this.TabText = "导航菜单";
            this.Text = "导航菜单";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvwNavigation;
        private System.Windows.Forms.ImageList ilNavigation;
    }
}