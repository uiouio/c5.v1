using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CTPPV5.Repository.Data
{
    public class SqlliteConnectionBuilder : IConnectionBuilder
    {
        #region IConnectionBuilder Members

        public IDbConnection Build(string connectionString)
        {
            return null;
        }

        #endregion
    }
}
