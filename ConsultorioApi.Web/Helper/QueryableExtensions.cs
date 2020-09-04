using ConsultorioApi.Entities;
using System.Linq;

namespace ConsultorioApi.Web
{
    /// <summary>
    /// Clase para mostrar una paginación de registros
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Metodo para devolver la cantidad total de registros
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="paginacion"></param>
        /// <returns></returns>
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, Paginacion paginacion)
        {
            return queryable.Skip((paginacion.Pagina - 1) * paginacion.CantidadRegistros)
                .Take(paginacion.CantidadRegistros);
        }
    }
}
