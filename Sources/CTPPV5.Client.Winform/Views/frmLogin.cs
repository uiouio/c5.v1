using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinFormsMvp;
using WinFormsMvp.Forms;
using WinFormsMvp.Binder;
using CTPPV5.Client.Winform.Presenters;
using CTPPV5.Client.Winform.Model;

namespace CTPPV5.Client.Winform.Views
{
    public interface ILoginView : IView<object>
    {
        event EventHandler LoginClicked;
        void OnLoginFinished();
    }

    [PresenterBinding(typeof(ILoginPresenter))]
    public partial class frmLogin : MvpForm, ILoginView
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        public event EventHandler LoginClicked;
        public void OnLoginFinished()
        {
            var loginResult = this.Model as LoginResult;
            if (loginResult.Result == Result.Success)
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (LoginClicked != null)
            {
                this.Model = new { Account = cboUser.Text.Trim(), Password = txtPWD.Text.Trim() };
                LoginClicked(this, new EventArgs());
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}