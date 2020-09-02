using ConsultorioApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsultorioApi.DataAccess
{
    public interface ICompaniaReporitorio
    {
        /// <summary>
        /// Inserta la información para una nueva compañia
        /// </summary>
        /// <param name="companiaInsert">Modelo con la información de la compañia</param>
        /// <param name="userId">Id del Usuario</param>
        /// <returns>Estatus del proceso <see cref="StatusProcessDB"/></returns>
        Task<StatusProcessDB> SetCompania(CompaniaInsert companiaInsert, string userId);

        /// <summary>
        /// Obtenemos un listado de las empresas
        /// </summary>
        /// <param name="companiaFiltro">Modelo de Objeto Tipo <see cref="CompaniaFiltro"/></param>
        /// <returns>Estatus del proceso <see cref="StatusProcessDB"/></returns>
        Task<List<CompaniaLista>> GetCompaniaList(CompaniaFiltro companiaFiltro);

        /// <summary>
        /// Actualiza la informacion de la empresa
        /// </summary>
        /// <param name="companiaEditar">Modelo con la información de la compañia</param>
        /// <param name="userId">Id del Usuario</param>
        /// <returns>Estatus del proceso <see cref="StatusProcessDB"/></returns>
        Task<StatusProcessDB> UpdateCompania(CompaniaEditar companiaEditar, string userId);

        /// <summary>
        /// Habilita o Inhabilita
        /// </summary>
        /// <param name="id">Id de la empresa</param>
        /// <param name="userId">Id del Usuario</param>
        /// <returns>Estatus del proceso <see cref="StatusProcessDB"/></returns>
        Task<StatusProcessDB> UpdateCompania(int id, bool Activo, string userId);
    }
}
