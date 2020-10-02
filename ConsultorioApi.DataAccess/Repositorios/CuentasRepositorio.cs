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
        /// Guardar la información de la persona en base de datos
        /// </summary>
        /// <param name="userId">Identificador de Usuario que modifica</param>
        /// <param name="persona">Obtejo de tipo <see cref="Persona"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcessDB"/></returns>
        public async Task<StatusProcessDB> SaveUser(string userId, Persona persona)
        {
            var people = JsonConvert.SerializeObject(persona);

            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@personaData", people, DbType.String, ParameterDirection.Input);
                parameters.Add("@userId", userId, DbType.String, ParameterDirection.Input);

                var records = await query.QueryMultipleAsync(
                    sql: "spGuardarPersona"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<StatusProcessDB>().FirstOrDefault();
            });
        }

        /// <summary>
        /// Modifica la información de la persona en base de datos
        /// </summary>
        /// <param name="userId">Identificador de Usuario que modifica</param>
        /// <param name="persona">Obtejo de tipo <see cref="Persona"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcessDB"/></returns>
        public async Task<StatusProcessDB> ModificaUsuario(string userId, Persona persona)
        {
            var people = JsonConvert.SerializeObject(persona);

            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@personaData", people, DbType.String, ParameterDirection.Input);
                parameters.Add("@userId", userId, DbType.String, ParameterDirection.Input);

                var records = await query.QueryMultipleAsync(
                    sql: "spModificarPersona"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<StatusProcessDB>().FirstOrDefault();
            });
        }

        /// <summary>
        /// Guarda la información del doctor
        /// </summary>
        /// <param name="doctor">Objeto tipo <see cref="Doctor"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcessDB"/></returns>
        public async Task<StatusProcessDB> GuardarMedico(Doctor doctor)
        {
            var doctorData = JsonConvert.SerializeObject(doctor);

            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@doctorData", doctorData, DbType.String, ParameterDirection.Input);

                var records = await query.QueryMultipleAsync(
                    sql: "spGuardarDoctor"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<StatusProcessDB>().FirstOrDefault();
            });
        }

        /// <summary>
        /// Guarda la relacion de Especialidades para el Medico
        /// </summary>
        /// <param name="doctor_id">Identificador del doctor</param>
        /// <param name="especialidades">Lista de Especialidades <see cref="Especialidad"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcessDB"/></returns>
        public async Task<StatusProcessDB> GuardarUsuarioEspecialidad(int doctor_id, List<Especialidad> especialidades)
        {
            var especialidadesData = JsonConvert.SerializeObject(especialidades);

            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@doctor_id", doctor_id, DbType.Int16, ParameterDirection.Input);
                parameters.Add("@especialidadesData", especialidadesData, DbType.String, ParameterDirection.Input);

                var records = await query.QueryMultipleAsync(
                    sql: "spGuardarDoctorEspecialidad"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<StatusProcessDB>().FirstOrDefault();
            });
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
                    sql: "spUsuarioData"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<User>().FirstOrDefault();
            });
        }

        /// <summary>
        /// Obtiene la información general de una lista de usuarios
        /// </summary>
        /// <param name="filter">Un objeto tipo <see cref="UserFiltro"/></param>
        /// <returns>Un listado de objetos tipo <see cref="User"/></returns>
        public async Task<List<User>> GetUser(UserFiltro filter)
        {
            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@email", filter.Email, DbType.String, ParameterDirection.Input);
                parameters.Add("@name", filter.Name, DbType.String, ParameterDirection.Input);
                parameters.Add("@estatusid", filter.EstatusId, DbType.String, ParameterDirection.Input);

                var records = await query.QueryMultipleAsync(
                    sql: "spUsuarioLista"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<User>().ToList();
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

        public async Task<List<Especialidad>> GetUserEspecialidad(int id)
        {
            return await WithConnection(async query =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@doctorId", id, DbType.String, ParameterDirection.Input);

                var records = await query.QueryMultipleAsync(
                    sql: "spDoctorEspecialidad"
                    , param: parameters
                    , commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                return records.Read<Especialidad>().ToList();
            });
        }
    }
}
