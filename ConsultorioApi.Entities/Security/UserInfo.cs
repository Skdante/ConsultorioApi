using System.Collections.Generic;

namespace ConsultorioApi.Entities
{
    public class UserInfo : UserAccess
    {
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public string Telefono { get; set; }
        public bool Estatus { get; set; }
        public string Imagen { get; set; }
        public string RolId { get; set; }
        public List<CompaniaLista> EmpresasRelacionadas { get; set; }
    }
}
