using System.Data.SqlClient;

namespace ConsultorioApi.DataAccess
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructor's SqlConnectionFactory
        /// </summary>
        /// <param name="connectionString">connectionString of the web.config</param>
        public ConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Create a connection of type SqlConnection, that receive the connectionString of constructor
        /// </summary>
        /// <returns>SqlConnection instance</returns>
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
