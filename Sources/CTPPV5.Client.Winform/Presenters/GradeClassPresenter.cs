using CTPPV5.Client.Winform.Views.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMvp;

namespace CTPPV5.Client.Winform.Presenters
{
    public interface IGradeClassPresenter : IModuleViewPresenter<IGradeClassView>
    {
    }

    public class GradeClassPresenter : ModuleViewPresenter<IGradeClassView>, IGradeClassPresenter
    {
        public GradeClassPresenter(IGradeClassView view)
            : base(view)
        {
 
        }
    }
}
