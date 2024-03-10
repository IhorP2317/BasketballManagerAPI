using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Security.Configurations {
    public class RoleEntityConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>> {
        private Guid _SuperAdminRoleId { get; set; }

        public RoleEntityConfiguration(Guid SuperAdminRoleId) {
            _SuperAdminRoleId = SuperAdminRoleId;
        }
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder) {
            builder.HasData(
                new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "User", NormalizedName = "User".ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString()},
                new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "Admin".ToUpper(),ConcurrencyStamp = Guid.NewGuid().ToString() },
                new IdentityRole<Guid> { Id = _SuperAdminRoleId, Name = "SuperAdmin", NormalizedName = "SuperAdmin".ToUpper(), ConcurrencyStamp = _SuperAdminRoleId.ToString()});
        }
    }
}
