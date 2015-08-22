using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Client.Winform
{
    public enum Result
    {
        Success,
        Failure
    }

    public enum ModuleType
    {
        SchoolInfo = 1,
        GradeClass = 2,
        StudentMgm = 3,
        TeacherMgm = 4,
        CardMachine = 5,
        SchoolMgm = 6,
        UserMgm = 7,
        PermissionMgm = 8,
        UpdatePwd = 9,
        SchoolNav = 100,
    }
}
