namespace ConsultorioApi.Entities
{
    /// <summary>
    /// Clase de Filtros de la Empresa
    /// </summary>
    public class CompaniaFiltro
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
        /// Activos
        /// </summary>
        public int Activos { get; set; }
        /// <summary>
        /// Total de Registros a consultar
        /// </summary>
        public Paginacion Paginacion {get; set;}
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public CompaniaFiltro()
        {
            Paginacion = new Paginacion();
        }
    }
}
