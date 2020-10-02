using System.Collections.Generic;

namespace ConsultorioApi.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido_paterno { get; set; }
        public string Apellido_materno { get; set; }
        public string Puesto { get; set; }
        public string Telefono { get; set; }
        public int Pais_Id { get; set; }
        public int Estado_Id { get; set; }
        public int Municipio_Id { get; set; }
        public string Colonia { get; set; }
        public string Calle { get; set; }
        public string Num_Exterior { get; set; }
        public string Num_Interior { get; set; }
        public bool Estatus { get; set; }
        public string Imagen { get; set; }
        public string RolId { get; set; }
        public string Descripcion { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Persona_Id { get; set; }
        public List<Especialidad> Especialidades { get; set; }
        public List<CompaniaLista> EmpresasRelacionadas { get; set; }
    }
}
