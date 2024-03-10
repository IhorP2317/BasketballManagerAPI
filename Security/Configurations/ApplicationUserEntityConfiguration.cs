using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Security.Models;
using Security.Settings;

namespace Security.Configurations {
    public class ApplicationUserEntityConfiguration: IEntityTypeConfiguration<ApplicationUser> {
       private readonly SuperAdminSettings _settings;

        public ApplicationUserEntityConfiguration(SuperAdminSettings  settings)
        {
           _settings = settings;
        }
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
           

            var appUser = new ApplicationUser {
                Id =Guid.Parse( _settings.Id),
                Email = _settings.Email,
                NormalizedEmail =_settings.Email.ToUpper(),
                EmailConfirmed = true,
                FirstName = _settings.FirstName,
                LastName = _settings.LastName,
                UserName = _settings.UserName,
                NormalizedUserName = _settings.UserName.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            appUser.PasswordHash = ph.HashPassword(appUser, _settings.Password);

            builder.HasData(appUser);
        }
    }
}
