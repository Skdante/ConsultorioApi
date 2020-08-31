using ConsultorioApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsultorioApi.Core
{
    public interface ICompania
    {
        Task<StatusProcess> CompaniaInsert(CompaniaInsert companiaInsert, string userId);
        Task<List<CompaniaLista>> GetCompaniaList();
    }
}
