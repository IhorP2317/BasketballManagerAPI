using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using Security.Settings;

namespace Security.Configurations {
    public class UserRoleEntityConfiguration:IEntityTypeConfiguration<IdentityUserRole<Guid>> {
      private readonly SuperAdminSettings _settings;

        public UserRoleEntityConfiguration(SuperAdminSettings settings)
        {
            _settings = settings;
        }
        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
           

            builder.HasData(new IdentityUserRole<Guid> {
                UserId = Guid.Parse(_settings.Id),
                RoleId = Guid.Parse(_settings.RoleId)
            });
        }
    }
}
