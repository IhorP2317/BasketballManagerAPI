using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Security.Configurations;
using Security.Models;

namespace Security.Data {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid,
        IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>> {
        private readonly  Guid SUPER_ADMIN_ID = Guid.NewGuid();
        private readonly Guid ROLE_ID = Guid.NewGuid();
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        
        public ApplicationDbContext() {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ApplicationUserEntityConfiguration(SUPER_ADMIN_ID));
            modelBuilder.ApplyConfiguration(new RoleEntityConfiguration(ROLE_ID));
            modelBuilder.ApplyConfiguration(new UserRoleEntityConfiguration(SUPER_ADMIN_ID, ROLE_ID));


        }
    }
}
