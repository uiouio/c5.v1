using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CTPPV5.Infrastructure.Cache;

namespace CTPPV5.Repository.Data
{
    public class SqlItem
    {
        private string readServer;
        private string writeServer;
        private IConnectionBuilder builder;
        private string rawConnectionString;
        public SqlItem(
            string name,
            string sql,
            string readServer,
            string writeServer,
            string rawConnectionString,
            IConnectionBuilder builder,
            ICache<string, object> cache)
        {
            this.Name = name;
            this.Sql = sql;
            this.Cache = cache;
            this.readServer = readServer;
            this.writeServer = writeServer;
            this.builder = builder;
            this.rawConnectionString = rawConnectionString;
        }
        public string Name { get; private set; }
        public string Sql { get; private set; }
        public ICache<string, object> Cache { get; private set; }
        public IDbConnection OpenReadConnection(string databaseName = "")
        {
            return OpenConnection(databaseName, readServer);
        }

        public IDbConnection OpenWriteConnection(string databaseName = "")
        {
            return OpenConnection(databaseName, writeServer);
        }

        public IDbConnection OpenConnection(string databaseName, string serverName)
        {
            var connectionString = string.Format(rawConnectionString,
                           !string.IsNullOrEmpty(databaseName) ? databaseName : string.Empty,
                           !string.IsNullOrEmpty(readServer) ? readServer : string.Empty);
            var connection = builder.Build(connectionString);
            connection.Open();
            return connection;
        }
    }
}
