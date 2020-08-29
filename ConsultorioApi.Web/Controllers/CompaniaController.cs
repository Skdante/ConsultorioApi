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
        /// Controlador de COmpañia
        /// </summary>
        /// <param name="_companiaCore">Interfaz tipo <see cref="ICompania"/></param>
        /// <param name="_userManager">Interfaz tipo <see cref="UserManager<ApplicationUser>"/></param>
        public CompaniaController(ICompania _companiaCore, UserManager<ApplicationUser> _userManager)
        {
            companiaCore = _companiaCore;
            userManager = _userManager;
        }

        /// <summary>
        /// Inserta la información de la compañia
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
    }
}