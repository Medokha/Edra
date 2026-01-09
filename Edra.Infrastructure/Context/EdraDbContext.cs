

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Edra.Infrastructure.Context
{
    public class EdraDbContext : IdentityDbContext<IdentityUser>
    {
        public EdraDbContext(DbContextOptions<EdraDbContext> option) :base(option) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


        // DbSet 


    }
}
