using System.Threading.Tasks;
using ConsultorioApi.Core;
using ConsultorioApi.Entities;
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
    public class CompaniaController : ControllerBase
    {
        private readonly ICompania companiaCore;

        /// <summary>
        /// Controlador de COmpañia
        /// </summary>
        /// <param name="_companiaCore">Interfaz tipo <see cref="ICompania"/></param>
        public CompaniaController(ICompania _companiaCore)
        {
            companiaCore = _companiaCore;
        }

        /// <summary>
        /// Inserta la información de la compañia
        /// </summary>
        /// <param name="compania">Modelo de Objeto Tipo <see cref="CompaniaInsert"/></param>
        /// <returns>Devuelve un objeto tipo <see cref="StatusProcess"/></returns>
        [HttpPost]
        public async Task<ActionResult<StatusProcess>> Post([FromBody] CompaniaInsert compania)
        {
            var result = await companiaCore.CompaniaInsert(compania).ConfigureAwait(false);
            if (result != null)
                return Ok(result);
            return StatusCode(500);
        }
    }
}