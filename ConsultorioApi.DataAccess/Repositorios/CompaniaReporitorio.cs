using Dapper;
using System.Threading.Tasks;
using ConsultorioApi.Entities;
using System.Data;
using Newtonsoft.Json;
using System.Linq;

namespace ConsultorioApi.DataAccess
{
    public class CompaniaReporitorio : BaseRepository, ICompaniaReporitorio
    {
        public CompaniaReporitorio(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        /// <summary>
        /// Inserta la información para una nueva compañia
        /// </summary>
        /// <param name="companiaInsert">Modelo con la información de la compañia</param>
        /// <param name="userId">Id del Usuario</param>
        /// <returns>Estatus del proceso <see cref="StatusProcessDB"/></returns>
        public async Task<StatusProcessDB> SetCompania(CompaniaInsert companiaInsert, string userId)
        {
            var compania = JsonConvert.SerializeObject(companiaInsert);

            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@companiaData", compania, DbType.String, ParameterDirection.Input);
                parameters.Add("@userId", userId, DbType.String, ParameterDirection.Input);

                var records = await query.QueryMultipleAsync(
                    sql: "CompaniaInsert"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<StatusProcessDB>().FirstOrDefault();
            });
        }
    }
}
