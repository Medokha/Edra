using Edra.Domain.Entities;
using Edra.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Edra.Infrastructure.Context
{
    public class EdraDbContext : IdentityDbContext<IdentityUser>
    {
        private readonly IUserResolveHandler _currentUser ;
        public EdraDbContext(DbContextOptions<EdraDbContext> option, IUserResolveHandler userResolve) :base(option) {
          _currentUser = userResolve;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Configure all entities inheriting from BaseEntity
            foreach (var entityType in builder.Model.GetEntityTypes()) {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)) {
                    // Set GUID to be generated on add
                    builder.Entity(entityType.ClrType)
                        .Property("Id")
                        .ValueGeneratedOnAdd();
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var currentUser = _currentUser.GetUserGuid();
            var currentTime = DateTime.UtcNow;

            foreach (var entry in entries) {
                if (entry.State == EntityState.Added) {
                    // Generate new GUID if not set
                    if (entry.Entity.Id == Guid.Empty) {
                        entry.Entity.Id = Guid.NewGuid();
                    }
                    entry.Entity.CreatedBy = currentUser;
                    entry.Entity.CreatedDate = currentTime;
                 
                } else if (entry.State == EntityState.Modified) {
                    entry.Entity.UpdatedBy = currentUser;
                    entry.Entity.UpdatedDate = currentTime;

                    // Prevent modification of Created fields and Id
                    entry.Property(x => x.Id).IsModified = false;
                    entry.Property(x => x.CreatedBy).IsModified = false;
                    entry.Property(x => x.CreatedDate).IsModified = false;
                }else if (entry.State == EntityState.Deleted) {
                    entry.Entity.IsDeletedBy = currentUser;
                    entry.Entity.IsDeletedDate = currentTime;
                }
            }
        }
        // DbSet 


    }
}
