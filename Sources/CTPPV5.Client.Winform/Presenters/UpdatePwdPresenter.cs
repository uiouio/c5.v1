using CTPPV5.Client.Winform.Views.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Client.Winform.Presenters
{
    public interface IUpdatePwdPresenter : IModuleViewPresenter<IUpdatePwdView>
    {
    }

    public class UpdatePwdPresenter : ModuleViewPresenter<IUpdatePwdView>, IUpdatePwdPresenter
    {
        public UpdatePwdPresenter(IUpdatePwdView view)
            : base(view)
        {

        }
    }
}
