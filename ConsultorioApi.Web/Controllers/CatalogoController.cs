using System.Collections.Generic;
using System.Threading.Tasks;
using ConsultorioApi.Core.Interfaces;
using ConsultorioApi.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ConsultorioApi.Web.Controllers
{
    /// <summary>
    /// Relacionado a la información general
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Catalogos de la aplicación")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CatalogoController : ControllerBase
    {
        private readonly ICatalogo _catalogoCore;

        /// <summary>
        /// Controlador de Catalogos
        /// </summary>
        /// <param name="catalogoCore">Interfaz tipo <see cref="ICatalogo"/></param>
        public CatalogoController(ICatalogo catalogoCore)
        {
            _catalogoCore = catalogoCore;
        }

        /// <summary>
        /// Obtenemos un listado de los estados
        /// </summary>
        /// <param name="PaisId">El identificador del pais</param>
        /// <returns>Devuelve un objeto tipo <see cref="Estado"/></returns>
        [HttpGet("Estados")]
        public async Task<ActionResult<List<Estado>>> Get(int PaisId)
        {
            var result = await _catalogoCore.GetEstados(PaisId);
            if (result != null)
            {
                return Ok(result);
            }
            return StatusCode(500);
        }

        /// <summary>
        /// Obtenemos un listado de los municipios por estado
        /// </summary>
        /// <param name="EstadoId">El identificador del estado</param>
        /// <returns>Devuelve un objeto tipo <see cref="Municipio"/></returns>
        [HttpGet("Municipios")]
        public async Task<ActionResult<List<Municipio>>> Municipíos(int EstadoId)
        {
            var result = await _catalogoCore.GetMunicipios(EstadoId);
            if (result != null)
            {
                return Ok(result);
            }
            return StatusCode(500);
        }

        /// <summary>
        /// Obtenemos un listado de las especialidades del doctor
        /// </summary>
        /// <returns>Devuelve un listado de especialidades tipo <see cref="Especialidad"/></returns>
        [HttpGet("Especialidades")]
        public async Task<ActionResult<List<Especialidad>>> Especialidades()
        {
            var result = await _catalogoCore.GetEspecialidades();
            if (result != null)
            {
                return Ok(result);
            }
            return StatusCode(500);
        }
    }
}
