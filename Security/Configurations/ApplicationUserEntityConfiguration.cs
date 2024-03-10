using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Security.Models;

namespace Security.Configurations {
    public class ApplicationUserEntityConfiguration: IEntityTypeConfiguration<ApplicationUser> {
        private Guid _SuperAdminId { get; set; }

        public ApplicationUserEntityConfiguration(Guid SuperAdminId)
        {
            _SuperAdminId = SuperAdminId;
        }
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
           

            var appUser = new ApplicationUser {
                Id = _SuperAdminId,
                Email = "mrsplash2356@gmail.com",
                EmailConfirmed = true,
                FirstName = "Ihor",
                LastName = "Paranchuk",
                UserName = "ipvsplash1117@gmail.com",
                NormalizedUserName = "IPVSPLASH1117@GMAIL.COM"
            };

            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            appUser.PasswordHash = ph.HashPassword(appUser, "2356_Sasa");

            builder.HasData(appUser);
        }
    }
}
