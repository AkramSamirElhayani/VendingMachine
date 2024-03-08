 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VendingMachine.Api.Identity
{
    public class ApplicationIdentityDbContext :IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
           : base(options)
        {
           // Database.Migrate();
        }
    }
}
