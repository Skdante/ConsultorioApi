using ConsultorioApi.Entities;
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
    }
}
