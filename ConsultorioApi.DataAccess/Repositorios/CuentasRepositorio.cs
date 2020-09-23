using ConsultorioApi.Entities;
using Dapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultorioApi.DataAccess
{
    public class CuentasRepositorio : BaseRepository, ICuentasRepositorio
    {
        public CuentasRepositorio(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        /// <summary>
        /// Inserta la relacion de la cuenta con las empresas seleccionadas
        /// </summary>
        /// <param name="userId">Identificador de Usuario</param>
        /// <param name="empresas">Lista de Empresas relacionadas <see cref="CompaniaLista"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcess"/></returns>
        public async Task<StatusProcessDB> InsertaRelacionEmpresaCuenta(string userId, List<CompaniaLista> empresas)
        {
            var compania = JsonConvert.SerializeObject(empresas);

            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@companiaData", compania, DbType.String, ParameterDirection.Input);
                parameters.Add("@userId", userId, DbType.String, ParameterDirection.Input);

                var records = await query.QueryMultipleAsync(
                    sql: "UserCompaniaInsert"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<StatusProcessDB>().FirstOrDefault();
            });
        }

        /// <summary>
        /// Obtiene los Roles existentes
        /// </summary>
        /// <returns>Listado del objeto <see cref="RolList"/></returns>
        public async Task<List<RolList>> GetRol()
        {
            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                var records = await query.QueryMultipleAsync(
                    sql: "RolList"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<RolList>().ToList();
            });
        }

        /// <summary>
        /// Obtiene la información del usuario
        /// </summary>
        /// <returns>Un objeto tipo <see cref="User"/></returns>
        public async Task<User> GetUser(string id)
        {
            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@userId", id, DbType.String, ParameterDirection.Input);

                var records = await query.QueryMultipleAsync(
                    sql: "UsuarioData"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<User>().FirstOrDefault();
            });
        }

        /// <summary>
        /// Obtiene la relación de las empresas con el usuario
        /// </summary>
        /// <returns>Un objeto tipo <see cref="User"/></returns>
        public async Task<List<CompaniaLista>> GetUserEmpresa(string id)
        {
            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@userId", id, DbType.String, ParameterDirection.Input);

                var records = await query.QueryMultipleAsync(
                    sql: "UsuarioEmpresa"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<CompaniaLista>().ToList();
            });
        }
    }
}
