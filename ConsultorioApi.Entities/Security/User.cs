using System.Collections.Generic;

namespace ConsultorioApi.Entities
{
    public class User
    {
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmar { get; set; }
        public string Telefono { get; set; }
        public string RolId { get; set; }
        public bool Estatus { get; set; }
        public string Imagen { get; set; }
        public List<CompaniaLista> EmpresasRelacionadas { get; set; }
    }
}
