using CTPPV5.Client.Winform.Views.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Client.Winform.Presenters
{
    public interface ISchoolInfoPresenter : IModuleViewPresenter<ISchoolInfoView>
    {
    }

    public class SchoolInfoPresenter : ModuleViewPresenter<ISchoolInfoView>, ISchoolInfoPresenter
    {
        public SchoolInfoPresenter(ISchoolInfoView view)
            : base(view)
        {

        }
    }
}
