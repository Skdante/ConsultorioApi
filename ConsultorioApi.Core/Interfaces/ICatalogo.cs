using ConsultorioApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsultorioApi.Core.Interfaces
{
    public interface ICatalogo
    {
        Task<List<Estado>> GetEstados(int paisId);
        Task<List<Municipio>> GetMunicipios(int paisId);
        Task<List<Especialidad>> GetEspecialidades();
    }
}
