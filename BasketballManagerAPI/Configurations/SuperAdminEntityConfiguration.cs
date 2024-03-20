using BasketballManagerAPI.Models;
using BasketballManagerAPI.Settings;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BasketballManagerAPI.Configurations {
    public class SuperAdminEntityConfiguration : IEntityTypeConfiguration<User> {
        private readonly SuperAdminSeedData _superAdminSeedData;

        public SuperAdminEntityConfiguration(SuperAdminSeedData superAdminSeedData) {
            _superAdminSeedData = superAdminSeedData;
        }

        public void Configure(EntityTypeBuilder<User> builder) {
            var appUser = new User {
                Id = Guid.Parse(_superAdminSeedData.SuperAdminId),
                Role = (Role)Enum.Parse(typeof(Role), _superAdminSeedData.SuperAdminRole, true),
                Email = _superAdminSeedData.Email,
                EmailConfirmed = true,
                FirstName = _superAdminSeedData.FirstName,
                LastName = _superAdminSeedData.LastName,
            };

            builder.HasData(appUser);
        }
    }
}
