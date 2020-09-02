using Dapper;
using System.Threading.Tasks;
using ConsultorioApi.Entities;
using System.Data;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

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

        /// <summary>
        /// Obtenemos la informacion de las empresas
        /// </summary>
        /// <param name="companiaFiltro">Modelo de Objeto Tipo <see cref="CompaniaFiltro"/></param>
        /// <returns>Lista de objetos tipo <see cref="CompaniaLista"/></returns>
        public async Task<List<CompaniaLista>> GetCompaniaList(CompaniaFiltro companiaFiltro)
        {
            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@companiaId", companiaFiltro.Compania_id, DbType.String, ParameterDirection.Input);
                parameters.Add("@nombre", companiaFiltro.Compania_nombre, DbType.String, ParameterDirection.Input);
                parameters.Add("@pais", companiaFiltro.Pais, DbType.String, ParameterDirection.Input);
                parameters.Add("@estado", companiaFiltro.Estado, DbType.String, ParameterDirection.Input);
                parameters.Add("@colonia", companiaFiltro.Colonia, DbType.String, ParameterDirection.Input);

                var records = await query.QueryMultipleAsync(
                    sql: "CompaniaList"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<CompaniaLista>().ToList();
            });
        }

        /// <summary>
        /// Actualiza la informacion de la empresa
        /// </summary>
        /// <param name="companiaEditar">Modelo con la información de la compañia</param>
        /// <param name="userId">Id del Usuario</param>
        /// <returns>Estatus del proceso <see cref="StatusProcessDB"/></returns>
        public async Task<StatusProcessDB> UpdateCompania(CompaniaEditar companiaEditar, string userId)
        {
            var compania = JsonConvert.SerializeObject(companiaEditar);

            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@companiaData", compania, DbType.String, ParameterDirection.Input);
                parameters.Add("@userId", userId, DbType.String, ParameterDirection.Input);

                var records = await query.QueryMultipleAsync(
                    sql: "CompaniaEditar"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<StatusProcessDB>().FirstOrDefault();
            });
        }

        /// <summary>
        /// Actualiza la informacion de la empresa
        /// </summary>
        /// <param name="companiaEditar">Modelo con la información de la compañia</param>
        /// <param name="userId">Id del Usuario</param>
        /// <returns>Estatus del proceso <see cref="StatusProcessDB"/></returns>
        public async Task<StatusProcessDB> UpdateCompania(int id, bool activo, string userId)
        {
            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id, DbType.String, ParameterDirection.Input);
                parameters.Add("@active", activo, DbType.Boolean, ParameterDirection.Input);
                parameters.Add("@userId", userId, DbType.String, ParameterDirection.Input);

                var records = await query.QueryMultipleAsync(
                    sql: "CompaniaActivo"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<StatusProcessDB>().FirstOrDefault();
            });
        }
    }
}
