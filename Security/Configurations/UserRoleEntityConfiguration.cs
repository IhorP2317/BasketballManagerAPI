using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Security.Configurations {
    public class UserRoleEntityConfiguration:IEntityTypeConfiguration<IdentityUserRole<Guid>> {
        private Guid _SuperAdminId { get; set; }
        private Guid _SuperAdminRoleId { get; set; }

        public UserRoleEntityConfiguration(Guid SuperAdminId, Guid SuperAdminRoleId) {
            _SuperAdminId = SuperAdminId;
            _SuperAdminRoleId = SuperAdminRoleId;
        }
        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
           

            builder.HasData(new IdentityUserRole<Guid> {
                UserId = _SuperAdminId,
                RoleId = _SuperAdminRoleId
            });
        }
    }
}
