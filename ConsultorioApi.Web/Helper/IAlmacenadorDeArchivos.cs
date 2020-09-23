using System.Threading.Tasks;

namespace ConsultorioApi.Web
{
    /// <summary>
    /// Interface IAlmacenadorDeArchivos
    /// </summary>
    public interface IAlmacenadorDeArchivos
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contenido"></param>
        /// <param name="extension"></param>
        /// <param name="nombreContenedor"></param>
        /// <param name="rutaArchivoActual"></param>
        /// <returns></returns>
        Task<string> EditarArchivo(byte[] contenido, string extension, string nombreContenedor, string rutaArchivoActual);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="nombreContenedor"></param>
        /// <returns></returns>
        Task EliminarArchivo(string ruta, string nombreContenedor);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contenido"></param>
        /// <param name="extension"></param>
        /// <param name="nombreContenedor"></param>
        /// <returns></returns>
        Task<string> GuardarArchivo(byte[] contenido, string extension, string nombreContenedor);
    }
}
