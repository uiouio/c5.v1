using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CTPPV5.Repository.Data
{
    public class MySqlConnectionBuilder : IConnectionBuilder
    {
        #region IConnectionBuilder Members

        public IDbConnection Build(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        #endregion
    }
}
