using CTPPV5.Client.Winform.Views.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Client.Winform.Presenters
{
    public interface IPermissionMgmPresenter : IModuleViewPresenter<IPermissionMgmView>
    {
    }

    public class PermissionMgmPresenter : ModuleViewPresenter<IPermissionMgmView>, IPermissionMgmPresenter
    {
        public PermissionMgmPresenter(IPermissionMgmView view)
            : base(view)
        {
 
        }
    }
}
