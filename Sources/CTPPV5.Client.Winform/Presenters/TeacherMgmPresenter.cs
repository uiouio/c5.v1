using CTPPV5.Client.Winform.Views.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Client.Winform.Presenters
{
    public interface ITeacherMgmPresenter : IModuleViewPresenter<ITeacherMgmView>
    {
    }

    public class TeacherMgmPresenter : ModuleViewPresenter<ITeacherMgmView>, ITeacherMgmPresenter
    {
        public TeacherMgmPresenter(ITeacherMgmView view)
            : base(view)
        {

        }
    }
}
