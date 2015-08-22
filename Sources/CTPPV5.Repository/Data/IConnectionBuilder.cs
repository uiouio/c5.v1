using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Repository.Data
{
    public interface IConnectionBuilder
    {
        IDbConnection Build(string connectionString);
    }
}
