using CTPPV5.Client.Winform.Views;
using CTPPV5.Infrastructure;
using CTPPV5.Infrastructure.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsMvp.Binder;
using Autofac;

namespace CTPPV5.Client.Winform
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool ret;
            Mutex mutex = new Mutex(true, Application.ProductName, out ret);
            if (ret)
            {
                ObjectHost.Setup(new Module[] { new WinformModule(), new LocalApiMockModule() });
                PresenterBinder.Factory = new AutofacPresenterFactory();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                frmLogin loginForm = new frmLogin();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new frmMain());
                }
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show(null, "有一个和本程序相同的应用程序正在运行，请不要同时运行多个本程序。\n\n这个程序即将退出。", Constants.PRODUCT_NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }
        }
    }
}
