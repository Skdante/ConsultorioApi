using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsultorioApi.DataAccess;
using ConsultorioApi.Entities;
using Serilog;

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
            StatusProcess statusProcess = new StatusProcess();
            
            try
            {
                statusProcess = await companiaRepositorio.SetCompania(companiaInsert, userId);

                if(!statusProcess.Estatus)
                    Log.Error("Metodo CompaniaInsert: {@companiaInsert}", companiaInsert, statusProcess);
            }
            catch (Exception ex)
            {
                Log.Error("Metodo CompaniaInsert: {@companiaInsert}", companiaInsert, statusProcess);
                statusProcess.Estatus = false;
                statusProcess.Mensaje = ex.Message;
            }
            return statusProcess;
        }

        /// <summary>
        /// Obtenemos la informacion de las empresas
        /// </summary>
        /// <param name="companiaFiltro">Modelo de Objeto Tipo <see cref="CompaniaFiltro"/></param>
        /// <returns>Lista de objetos tipo <see cref="CompaniaLista"/></returns>
        public async Task<List<CompaniaLista>> GetCompaniaList(CompaniaFiltro companiaFiltro)
        {
            List<CompaniaLista> companiaListas;

            try
            {
                companiaListas = await companiaRepositorio.GetCompaniaList(companiaFiltro);
            }
            catch (Exception ex)
            {
                Log.Error("Metodo GetCompaniaList: {@companiaFiltro}", companiaFiltro, ex.Message);
                companiaListas = new List<CompaniaLista>();
            }
            return companiaListas;
        }

        /// <summary>
        /// Actualiza la informacion de la empresa
        /// </summary>
        /// <param name="companiaEditar">Modelo con la información de la compañia</param>
        /// <param name="userId">Id del Usuario</param>
        /// <returns>Estatus del proceso <see cref="StatusProcess"/></returns>
        public async Task<StatusProcess> GetCompaniaEdit(CompaniaEditar companiaEditar, string userId)
        {
            StatusProcess statusProcess = new StatusProcess();

            try
            {
                statusProcess = await companiaRepositorio.UpdateCompania(companiaEditar, userId);

                if (!statusProcess.Estatus)
                    Log.Error("Metodo GetCompaniaEdit: {@companiaEditar}", companiaEditar, statusProcess, userId);
            }
            catch (Exception ex)
            {
                Log.Error("Metodo GetCompaniaEdit: {@companiaEditar}", companiaEditar, userId, ex.Message);
                statusProcess.Estatus = false;
                statusProcess.Mensaje = ex.Message;
            }
            return statusProcess;
        }

        /// <summary>
        /// Habilita o Inhabilita la empresa
        /// </summary>
        /// <param name="id">Id de la empresa</param>
        /// <param name="activo">Activa o inactiva la empresa</param>
        /// <returns>Estatus del proceso <see cref="StatusProcess"/></returns>
        public async Task<StatusProcess> FetchCompaniaInhabilitar(int id, bool activo, string userId)
        {
            StatusProcess statusProcess = new StatusProcess();

            try
            {
                statusProcess = await companiaRepositorio.UpdateCompania(id, activo, userId);

                if (!statusProcess.Estatus)
                    Log.Error("Metodo FetchCompaniaInhabilitar: {@id}", id, statusProcess, userId);
            }
            catch (Exception ex)
            {
                Log.Error("Metodo FetchCompaniaInhabilitar: {@id}", id, userId, ex.Message);
                statusProcess.Estatus = false;
                statusProcess.Mensaje = ex.Message;
            }
            return statusProcess;
        }
    }
}
