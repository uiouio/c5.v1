using CTPPV5.Client.Winform.Views.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Client.Winform.Presenters
{
    public interface IUserMgmPresenter : IModuleViewPresenter<IUserMgmView>
    {
    }

    public class UserMgmPresenter : ModuleViewPresenter<IUserMgmView>, IUserMgmPresenter
    {
        public UserMgmPresenter(IUserMgmView view)
            : base(view)
        {

        }
    }
}
