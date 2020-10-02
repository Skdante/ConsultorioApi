namespace ConsultorioApi.Entities
{
    public class Persona
    {
        public int Persona_Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido_Paterno { get; set; }
        public string Apellido_Materno { get; set; }
        public string Telefono_Contacto { get; set; }
        public int Pais_Id { get; set; }
        public int Estado_Id { get; set; }
        public int Municipio_Id { get; set; }
        public string Colonia { get; set; }
        public string Calle { get; set; }
        public string Num_Exterior { get; set; }
        public string Num_Interior { get; set; }
        public string Imagen { get; set; }
    }
}
