using System;

namespace ConsultorioApi.Entities
{
    /// <summary>
    /// Clase CompaniaDto
    /// </summary>
    public class CompaniaDto
    {
        /// <summary>
        /// Identificacador de la compañia
        /// </summary>
        public int Compania_id { get; set; }
        /// <summary>
        /// Nombre de la compañia
        /// </summary>
        public string Compania_nombre { get; set; }
        /// <summary>
        /// Pais
        /// </summary>
        public string Pais { get; set; }
        /// <summary>
        /// Estado
        /// </summary>
        public string Estado { get; set; }
        /// <summary>
        /// Colonia
        /// </summary>
        public string Colonia { get; set; }
        /// <summary>
        /// Calle
        /// </summary>
        public string Calle { get; set; }
        /// <summary>
        /// Numero exterior
        /// </summary>
        public string Numero_exterior { get; set; }
        /// <summary>
        /// Numero interior
        /// </summary>
        public string Numero_interior { get; set; }
        /// <summary>
        /// Fecha de creación inicial
        /// </summary>
        public DateTime Fecha_inicial { get; set; }
        /// <summary>
        /// Fecha de ultima modificacion de los datos
        /// </summary>
        public DateTime Fecha_modificacion { get; set; }
        /// <summary>
        /// Usuario quien creo el informe
        /// </summary>
        public string Usuario_inicial { get; set; }
        /// <summary>
        ///  Ultimo usuario que modifico el informe
        /// </summary>
        public string Usuario_modificacion { get; set; }
    }
}
