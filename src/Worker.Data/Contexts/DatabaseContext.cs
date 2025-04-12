using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Worker.Data.Contexts
{
    public class DatabaseContext
    {

        private readonly DbConnectionStringBuilder _dbConnectionStringBuilder;

        public DatabaseContext(string connString)
        {
            _dbConnectionStringBuilder = new DbConnectionStringBuilder { ConnectionString = connString };
        }

        internal IDbConnection GetConnection()
        {
            DbConnection conn = SqlClientFactory.Instance.CreateConnection();
            conn.ConnectionString = _dbConnectionStringBuilder.ConnectionString;
            return conn;
        }
    }
}
