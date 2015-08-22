using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Models;

namespace CTPPV5.Client.Winform.Model
{
    public static class SchoolContext
    {
        const string CONTEXT_NAME = "SchoolContext_School";
        public static School Get()
        {
            School school = null;
            var data = AppDomain.CurrentDomain.GetData(CONTEXT_NAME);
            if (data != null)
                school = data as School;
            return school;
        }

        public static void Bind(this School school)
        {
            AppDomain.CurrentDomain.SetData(CONTEXT_NAME, school);
        }

        public static void Unbind(this School school)
        {
            var getSchool = Get();
            if (getSchool != null)
                AppDomain.CurrentDomain.SetData(CONTEXT_NAME, null);
        }
    }
}
