using System.Data.SqlClient;

namespace ConsultorioApi.DataAccess
{
    /// <summary>
    /// Interface IConnectionFactory
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Method CreateConnection <see cref="DataAccess.Connections.SqlConnectionFactory.CreateConnection"/>
        /// </summary>
        /// <returns>SqlConnection <see cref="SqlConnection"/></returns>
        SqlConnection CreateConnection();
    }
}
