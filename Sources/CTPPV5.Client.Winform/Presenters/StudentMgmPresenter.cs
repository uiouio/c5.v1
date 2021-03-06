﻿using CTPPV5.Client.Winform.Views.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Client.Winform.Presenters
{
    public interface IStudentMgmPresenter : IModuleViewPresenter<IStudentMgmView>
    {
    }

    public class StudentMgmPresenter : ModuleViewPresenter<IStudentMgmView>, IStudentMgmPresenter
    {
        public StudentMgmPresenter(IStudentMgmView view)
            : base(view)
        {

        }
    }
}
