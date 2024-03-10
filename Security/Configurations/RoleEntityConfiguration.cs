using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Security.Settings;

namespace Security.Configurations {
    public class RoleEntityConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>> {
        private readonly SuperAdminSettings _settings;


        public RoleEntityConfiguration(SuperAdminSettings settings) {
            _settings = settings;
        }
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder) {
            builder.HasData(
                new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "User", NormalizedName = "User".ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString()},
                new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "Admin".ToUpper(),ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole<Guid> { Id = Guid.Parse(_settings.RoleId), Name = "SuperAdmin", NormalizedName = "SuperAdmin".ToUpper(), ConcurrencyStamp = _settings.RoleId.ToString()});
        }
    }
}
