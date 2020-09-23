using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsultorioApi.Web
{
    /// <summary>
    /// Clase AlmacenadorArchivosLocal
    /// </summary>
    public class AlmacenadorArchivosLocal : IAlmacenadorDeArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        /// <param name="httpContextAccessor"></param>
        public AlmacenadorArchivosLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Edita el archivo o imagen descargado
        /// </summary>
        /// <param name="contenido"></param>
        /// <param name="extension"></param>
        /// <param name="nombreContenedor"></param>
        /// <param name="rutaArchivoActual"></param>
        /// <returns></returns>
        public async Task<string> EditarArchivo(byte[] contenido, string extension, string nombreContenedor, string rutaArchivoActual)
        {
            if (!string.IsNullOrEmpty(rutaArchivoActual))
            {
                await EliminarArchivo(rutaArchivoActual, nombreContenedor);
            }

            return await GuardarArchivo(contenido, extension, nombreContenedor);
        }

        /// <summary>
        /// Elimina el archivo
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="nombreContenedor"></param>
        /// <returns></returns>
        public Task EliminarArchivo(string ruta, string nombreContenedor)
        {
            var filename = Path.GetFileName(ruta);
            string directorioArchivo = Path.Combine(env.WebRootPath, nombreContenedor, filename);
            if (File.Exists(directorioArchivo))
            {
                File.Delete(directorioArchivo);
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// Guarda el archivo
        /// </summary>
        /// <param name="contenido"></param>
        /// <param name="extension"></param>
        /// <param name="nombreContenedor"></param>
        /// <returns></returns>
        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string nombreContenedor)
        {
            var filename = $"{Guid.NewGuid()}.{extension}";
            string folder = Path.Combine(env.WebRootPath, nombreContenedor);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string rutaGuardado = Path.Combine(folder, filename);
            await File.WriteAllBytesAsync(rutaGuardado, contenido);

            var urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var rutaParaBD = Path.Combine(urlActual, nombreContenedor, filename);
            return rutaParaBD;
        }
    }
}
