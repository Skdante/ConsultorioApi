using ConsultorioApi.Entities;
using System.Threading.Tasks;

namespace ConsultorioApi.DataAccess
{
    public interface ICompaniaReporitorio
    {
        /// <summary>
        /// Inserta la informacion de la Compañia
        /// </summary>
        /// <returns></returns>
        Task<StatusProcessDB> SetCompania(CompaniaInsert companiaInsert);
    }
}
