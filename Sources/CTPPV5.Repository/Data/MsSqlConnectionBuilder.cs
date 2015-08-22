using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Repository.Data
{
    public class MsSqlConnectionBuilder : IConnectionBuilder
    {
        #region IConnectionBuilder Members

        public System.Data.IDbConnection Build(string connectionString)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
