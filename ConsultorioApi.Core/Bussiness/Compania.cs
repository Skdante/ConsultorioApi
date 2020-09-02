using System.Collections.Generic;
using System.Threading.Tasks;
using ConsultorioApi.DataAccess;
using ConsultorioApi.Entities;

namespace ConsultorioApi.Core
{
    public class Compania : ICompania
    {
        private readonly ICompaniaReporitorio companiaRepositorio;

        public Compania(ICompaniaReporitorio _companiaRepositorio)
        {
            companiaRepositorio = _companiaRepositorio;
        }

        /// <summary>
        /// Inserta la información de la compañia
        /// </summary>
        /// <param name="companiaInsert">Modelo de Objeto tipo <see cref="CompaniaInsert"/></param>
        /// <param name="userId">Id del usuario</param>
        /// <returns>Modelo de Objeto tipo <see cref="StatusProcess"/></returns>
        public async Task<StatusProcess> CompaniaInsert(CompaniaInsert companiaInsert, string userId)
        {
            return await companiaRepositorio.SetCompania(companiaInsert, userId);
        }

        /// <summary>
        /// Obtenemos la informacion de las empresas
        /// </summary>
        /// <param name="companiaFiltro">Modelo de Objeto Tipo <see cref="CompaniaFiltro"/></param>
        /// <returns>Lista de objetos tipo <see cref="CompaniaLista"/></returns>
        public async Task<List<CompaniaLista>> GetCompaniaList(CompaniaFiltro companiaFiltro)
        {
            return await companiaRepositorio.GetCompaniaList(companiaFiltro);
        }

        /// <summary>
        /// Actualiza la informacion de la empresa
        /// </summary>
        /// <param name="companiaEditar">Modelo con la información de la compañia</param>
        /// <param name="userId">Id del Usuario</param>
        /// <returns>Estatus del proceso <see cref="StatusProcess"/></returns>
        public async Task<StatusProcess> GetCompaniaEdit(CompaniaEditar companiaEditar, string userId)
        {
            return await companiaRepositorio.UpdateCompania(companiaEditar, userId);
        }

        /// <summary>
        /// Habilita o Inhabilita la empresa
        /// </summary>
        /// <param name="id">Id de la empresa</param>
        /// <param name="activo">Activa o inactiva la empresa</param>
        /// <returns>Estatus del proceso <see cref="StatusProcess"/></returns>
        public async Task<StatusProcess> FetchCompaniaInhabilitar(int id, bool activo, string userId)
        {
            return await companiaRepositorio.UpdateCompania(id, activo, userId);
        }
    }
}
