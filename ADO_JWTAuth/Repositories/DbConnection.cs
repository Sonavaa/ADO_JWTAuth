using Microsoft.Data.SqlClient;

namespace ADO_JWTAuth.Repositories
{
    public abstract class DbConnection
    {
        private readonly string _connectionString;

        protected DbConnection(IConfiguration connectionString)
        {
            _connectionString = connectionString.GetConnectionString("ConnectionString");
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        protected SqlCommand CreateCommand(string SQLCommand, SqlConnection connection) 
        {
            return new SqlCommand(SQLCommand, connection);
        }
    }
}
