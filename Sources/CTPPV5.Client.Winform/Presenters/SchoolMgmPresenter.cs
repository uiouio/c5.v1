using CTPPV5.Client.Winform.Views.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Client.Winform.Presenters
{
    public interface ISchoolMgmPresenter : IModuleViewPresenter<ISchoolMgmView>
    {
    }

    public class SchoolMgmPresenter : ModuleViewPresenter<ISchoolMgmView>, ISchoolMgmPresenter
    {
        public SchoolMgmPresenter(ISchoolMgmView view)
            : base(view)
        {

        }
    }
}
