using ConsultorioApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsultorioApi.DataAccess
{
    public interface ICuentasRepositorio
    {
        /// <summary>
        /// Guardar la información de la persona en base de datos
        /// </summary>
        /// <param name="userId">Identificador de Usuario que modifica</param>
        /// <param name="persona">Obtejo de tipo <see cref="Persona"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcessDB"/></returns>
        Task<StatusProcessDB> SaveUser(string userId, Persona persona);

        /// <summary>
        /// Modifica la información de la persona en base de datos
        /// </summary>
        /// <param name="userId">Identificador de Usuario que modifica</param>
        /// <param name="persona">Obtejo de tipo <see cref="Persona"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcessDB"/></returns>
        Task<StatusProcessDB> ModificaUsuario(string userId, Persona persona);

        /// <summary>
        /// Guarda la información del doctor
        /// </summary>
        /// <param name="doctor">Objeto tipo <see cref="Doctor"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcessDB"/></returns>
        Task<StatusProcessDB> GuardarMedico(Doctor doctor);

        /// <summary>
        /// Guarda la relacion de Especialidades para el Medico
        /// </summary>
        /// <param name="doctor_id">Identificador del doctor</param>
        /// <param name="especialidades">Lista de Especialidades <see cref="Especialidad"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcessDB"/></returns>
        Task<StatusProcessDB> GuardarUsuarioEspecialidad(int doctor_id, List<Especialidad> especialidades);

        /// <summary>
        /// Inserta la relacion de la cuenta con las empresas seleccionadas
        /// </summary>
        /// <param name="userId">Identificador de Usuario</param>
        /// <param name="empresas">Lista de Empresas relacionadas <see cref="CompaniaLista"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcessDB"/></returns>
        Task<StatusProcessDB> InsertaRelacionEmpresaCuenta(string userId, List<CompaniaLista> empresas);

        /// <summary>
        /// Obtiene los Roles existentes
        /// </summary>
        /// <returns>Listado del objeto <see cref="RolList"/></returns>
        Task<List<RolList>> GetRol();

        /// <summary>
        /// Obtiene la información del usuario
        /// </summary>
        /// <returns>Un objeto tipo <see cref="User"/></returns>
        Task<User> GetUser(string id);

        /// <summary>
        /// Obtiene listado de usuarios por filtro
        /// </summary>
        /// <param name="filter">Un objeto de filtro tipo <see cref="UserFiltro"/></param>
        /// <returns>Un lista de usuarios tipo <see cref="User"/></returns>
        Task<List<User>> GetUser(UserFiltro filter);

        /// <summary>
        /// Obtiene la relación de las empresas con el usuario
        /// </summary>
        /// <returns>Un objeto tipo <see cref="User"/></returns>
        Task<List<CompaniaLista>> GetUserEmpresa(string id);

        /// <summary>
        /// Obtiene las especialidades por usuario
        /// </summary>
        /// <param name="id">Identificador del doctor</param>
        /// <returns>Listado de especialidades <see cref="Especialidad"/></returns>
        Task<List<Especialidad>> GetUserEspecialidad(int id);
    }
}
