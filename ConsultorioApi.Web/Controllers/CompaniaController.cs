using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsultorioApi.Core;
using ConsultorioApi.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ConsultorioApi.Web.Controllers
{
    /// <summary>
    /// Relacionado a la información de la empresa
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Relacionado a la información de la empresa")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CompaniaController : ControllerBase
    {
        private readonly ICompania companiaCore;
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        /// Controlador de Compañia
        /// </summary>
        /// <param name="_companiaCore">Interfaz tipo <see cref="ICompania"/></param>
        /// <param name="_userManager">Interfaz tipo UserManager</param>
        public CompaniaController(ICompania _companiaCore, UserManager<ApplicationUser> _userManager)
        {
            companiaCore = _companiaCore;
            userManager = _userManager;
        }

        /// <summary>
        /// Inserta la información de la empresa
        /// </summary>
        /// <param name="compania">Modelo de Objeto Tipo <see cref="CompaniaInsert"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcess"/></returns>
        [HttpPost]
        public async Task<ActionResult<StatusProcess>> Post([FromBody] CompaniaInsert compania)
        {
            var user = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            var result = await companiaCore.CompaniaInsert(compania, user.Id).ConfigureAwait(false);
            if (result != null)
                return Ok(result);
            return StatusCode(500);
        }

        /// <summary>
        /// Obtenemos un listado de las empresas
        /// </summary>
        /// <param name="companiaFiltro">Modelo de Objeto Tipo <see cref="CompaniaFiltro"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcess"/></returns>
        [HttpPost("Consultar")]
        public async Task<ActionResult<List<CompaniaLista>>> Post([FromBody] CompaniaFiltro companiaFiltro)
        {
            var result = await companiaCore.GetCompaniaList(companiaFiltro);
            if (result != null)
            {
                var quaryable = result.AsQueryable();
                await HttpContext.InsertaParametrosPaginacionEnRespuesta(quaryable, companiaFiltro.Paginacion.CantidadRegistros);
                result = quaryable.Paginar(companiaFiltro.Paginacion).ToList();
                return Ok(result);
            }
            return StatusCode(500);
        }

        /// <summary>
        /// Editamos la informacion de una empresa
        /// </summary>
        /// <param name="companiaEditar">Modelo de Objeto Tipo <see cref="CompaniaEditar"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcess"/></returns>
        [HttpPut("Editar")]
        public async Task<ActionResult<StatusProcess>> Put([FromBody] CompaniaEditar companiaEditar)
        {
            var user = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            var result = await companiaCore.GetCompaniaEdit(companiaEditar, user.Id).ConfigureAwait(false);
            if (result != null)
                return Ok(result);
            return StatusCode(500);
        }

        /// <summary>
        /// Habilitamos o inhabilitamos la empresa
        /// </summary>
        /// <param name="id">Id de la empresa a inhabilitar</param>
        /// <param name="activo">Activa o desactiva la empresa</param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcess"/></returns>
        [HttpPatch("Inhabilitar")]
        public async Task<ActionResult<StatusProcess>> Fetch(int id, bool activo)
        {
            var user = await userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            var result = await companiaCore.FetchCompaniaInhabilitar(id, activo, user.Id).ConfigureAwait(false);
            if (result != null)
                return Ok(result);
            return StatusCode(500);
        }
    }
}