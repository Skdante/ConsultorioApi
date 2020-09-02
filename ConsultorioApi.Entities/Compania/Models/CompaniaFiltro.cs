namespace ConsultorioApi.Entities
{
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
    }
}
