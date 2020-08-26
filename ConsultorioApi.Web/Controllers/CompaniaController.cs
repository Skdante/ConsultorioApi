using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsultorioApi.Core;
using ConsultorioApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsultorioApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniaController : ControllerBase
    {
        private readonly ICompania companiaCore;

        public CompaniaController(ICompania _companiaCore)
        {
            companiaCore = _companiaCore;
        }

        /// <summary>
        /// Inserta la información de la compañia
        /// </summary>
        /// <param name="compania">Modelo de Objeto Tipo <see cref="CompaniaInsert"/></param>
        [HttpPost]
        public async Task<ActionResult<CompaniaInsert>> Post([FromBody] CompaniaInsert compania)
        {
            var result = await companiaCore.CompaniaInsert(compania).ConfigureAwait(false);
            if (result != null)
                return Ok(result);
            return StatusCode(500);
        }
    }
}