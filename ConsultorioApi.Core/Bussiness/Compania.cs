using System;
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
        /// <returns>Lista de objetos tipo <see cref="CompaniaLista"/></returns>
        public async Task<List<CompaniaLista>> GetCompaniaList()
        {
            return await companiaRepositorio.GetCompaniaList();
        }
    }
}
