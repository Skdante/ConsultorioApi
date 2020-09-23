using ConsultorioApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsultorioApi.DataAccess
{
    public interface ICuentasRepositorio
    {
        /// <summary>
        /// Inserta la relacion de la cuenta con las empresas seleccionadas
        /// </summary>
        /// <param name="userId">Identificador de Usuario</param>
        /// <param name="empresas">Lista de Empresas relacionadas <see cref="CompaniaLista"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcess"/></returns>
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
        /// Obtiene la relación de las empresas con el usuario
        /// </summary>
        /// <returns>Un objeto tipo <see cref="User"/></returns>
        Task<List<CompaniaLista>> GetUserEmpresa(string id);
    }
}
