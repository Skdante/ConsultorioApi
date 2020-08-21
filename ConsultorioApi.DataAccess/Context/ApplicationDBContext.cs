using ConsultorioApi.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConsultorioApi.DataAccess
{
    public class ApplicationDBContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            :base(options)
        {

        }
    }
}
