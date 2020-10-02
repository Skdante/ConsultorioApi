using ConsultorioApi.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultorioApi.Core
{
    public interface ICuentas
    {
        /// <summary>
        /// Guarda el usuario
        /// </summary>
        /// <param name="userId">Identificador de Usuario</param>
        /// <param name="usuario">Objeto tipo <see cref="UserInfo"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcessDB"/></returns>
        Task<StatusProcessDB> SaveUser(string userId, UserInfo usuario);

        /// <summary>
        /// Modifica el usuario
        /// </summary>
        /// <param name="userId">Identificador de Usuario</param>
        /// <param name="usuario">Objeto tipo <see cref="UserInfo"/></param>
        /// <param name="personaId">Identificador de la persona</param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcess"/></returns>
        Task<StatusProcess> ModificarUsuario(string userId, UserInfo usuario, int personaId);

        /// <summary>
        /// Inserta la relacion de la cuenta con las empresas seleccionadas
        /// </summary>
        /// <param name="userId">Identificador de Usuario</param>
        /// <param name="empresas">Lista de Empresas relacionadas <see cref="CompaniaInsert"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcess"/></returns>
        Task<StatusProcess> InsertaRelacionEmpresaCuenta(string userId, List<CompaniaLista> empresas);

        /// <summary>
        /// COnstruye el token en base a la informacion de usuario
        /// </summary>
        /// <param name="userInfo">Modelo tipo <see cref="UserInfo"/></param>
        /// <param name="roles">Listado de roles del usuario</param>
        /// <param name="username">Username del usuario</param>
        /// <param name="jwtKey">Llave del token</param>
        /// <returns>Modelo tipo <see cref="UserToken"/></returns>
        UserToken BuildToken(UserInfo userInfo, IList<string> roles, string username, string jwtKey);

        /// <summary>
        /// Filtra la información del usuario
        /// </summary>
        /// <param name="applicationUsers">Listado del objeto <see cref="ApplicationUser"/></param>
        /// <param name="userFilter">Modelo tipo <see cref="UserFiltro"/></param>
        /// <returns>Listado del objeto <see cref="UserList"/></returns>
        Task<List<UserList>> FilterUser(IQueryable<ApplicationUser> applicationUsers, UserFiltro userFilter);

        /// <summary>
        /// Obtiene información de un usuario
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <returns>Objeto tipo <see cref="Entities.User"/></returns>
        Task<User> User(string id);

        /// <summary>
        /// Obtiene los Roles existentes
        /// </summary>
        /// <returns>Listado del objeto <see cref="RolList"/></returns>
        Task<List<RolList>> GetRol();
    }
}
