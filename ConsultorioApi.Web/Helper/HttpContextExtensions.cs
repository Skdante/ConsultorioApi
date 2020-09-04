using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultorioApi.Web
{
    /// <summary>
    /// Extención de la clase HttpContext
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Insertar los parametros de la paginación
        /// </summary>
        /// <typeparam name="t">Cualquier Objeto</typeparam>
        /// <param name="context">Objeto tipo <see cref="HttpContext"/></param>
        /// <param name="queryable">Objeto tipo <see cref="IQueryable"/></param>
        /// <param name="cantidadRegistrosMostrar">cantidad de objetos</param>
        /// <returns></returns>
        public async static Task InsertaParametrosPaginacionEnRespuesta<t>(this HttpContext context,
            IQueryable<t> queryable, int cantidadRegistrosMostrar)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }

            double conteo = queryable.Count();
            double totalPaginas = Math.Ceiling(conteo / cantidadRegistrosMostrar);
            context.Response.Headers.Add("conteo", conteo.ToString());
            context.Response.Headers.Add("totalPaginas", totalPaginas.ToString());
        }
    }
}
