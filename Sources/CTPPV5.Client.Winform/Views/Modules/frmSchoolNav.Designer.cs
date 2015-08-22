namespace CTPPV5.Client.Winform.Views.Modules
{
    partial class frmSchoolNav
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSchoolNav));
            this.tvSchoolNav = new System.Windows.Forms.TreeView();
            this.ilSchoolNav = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // tvSchoolNav
            // 
            this.tvSchoolNav.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSchoolNav.ImageIndex = 0;
            this.tvSchoolNav.ImageList = this.ilSchoolNav;
            this.tvSchoolNav.Location = new System.Drawing.Point(0, 0);
            this.tvSchoolNav.Name = "tvSchoolNav";
            this.tvSchoolNav.SelectedImageIndex = 0;
            this.tvSchoolNav.Size = new System.Drawing.Size(246, 677);
            this.tvSchoolNav.TabIndex = 0;
            this.tvSchoolNav.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvSchoolNav_NodeMouseClick);
            // 
            // ilSchoolNav
            // 
            this.ilSchoolNav.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSchoolNav.ImageStream")));
            this.ilSchoolNav.TransparentColor = System.Drawing.Color.Transparent;
            this.ilSchoolNav.Images.SetKeyName(0, "root");
            this.ilSchoolNav.Images.SetKeyName(1, "category");
            this.ilSchoolNav.Images.SetKeyName(2, "teacher.png");
            // 
            // frmSchoolNav
            // 
            this.AllowEndUserDocking = false;
            this.AutoHidePortion = 0.18D;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 677);
            this.Controls.Add(this.tvSchoolNav);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight;
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmSchoolNav";
            this.TabText = "学校导航栏";
            this.Text = "学校导航栏";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvSchoolNav;
        private System.Windows.Forms.ImageList ilSchoolNav;
    }
}