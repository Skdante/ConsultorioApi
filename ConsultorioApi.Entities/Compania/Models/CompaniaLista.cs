namespace ConsultorioApi.Entities
{
    /// <summary>
    /// Compania List
    /// </summary>
    public class CompaniaLista
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
        /// Indica si esta activo o no
        /// </summary>
        public bool Activo { get; set; }
    }
}
