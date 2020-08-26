using System;
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
        /// <returns>Modelo de Objeto tipo <see cref="StatusProcess"/></returns>
        public async Task<StatusProcess> CompaniaInsert(CompaniaInsert companiaInsert)
        {
            return await companiaRepositorio.SetCompania(companiaInsert);
        }
    }
}
