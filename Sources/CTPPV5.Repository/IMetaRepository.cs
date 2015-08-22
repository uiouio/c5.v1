using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Models;

namespace CTPPV5.Repository
{
    public interface IMetaRepository
    {
        void AddOrUpdateSchool(School school, Action<bool> callback);
        SchoolCollection GetAllSchools();
    }
}
