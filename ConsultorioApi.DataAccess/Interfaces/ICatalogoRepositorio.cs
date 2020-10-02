using ConsultorioApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsultorioApi.DataAccess
{
    public interface ICatalogoRepositorio
    {
        Task<List<EstadoDB>> GetEstado(int paisId);
        Task<List<Municipio>> GetMunicipio(int estadoId);
        Task<List<Especialidad>> GetEspecialidad();
    }
}
