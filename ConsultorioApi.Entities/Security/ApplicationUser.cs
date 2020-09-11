using Microsoft.AspNetCore.Identity;

namespace ConsultorioApi.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public bool? IsEnabled { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
    }
}
