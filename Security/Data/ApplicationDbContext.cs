using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Security.Configurations;
using Security.Models;
using Security.Settings;

namespace Security.Data {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid,
        IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>> {
        private readonly SuperAdminSettings _settings;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptions<SuperAdminSettings> settings)
            : base(options)
        {
            _settings = settings.Value;
        }

        
       
        protected override void OnModelCreating(ModelBuilder modelBuilder ) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ApplicationUserEntityConfiguration(_settings));
            modelBuilder.ApplyConfiguration(new RoleEntityConfiguration(_settings));
            modelBuilder.ApplyConfiguration(new UserRoleEntityConfiguration(_settings));


        }
    }
}
