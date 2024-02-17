using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class StaffExperienceEntityConfiguration:BaseEntityConfiguration<StaffExperience> {
        public override void Configure(EntityTypeBuilder<StaffExperience> builder)
        {
            base.Configure(builder);
            builder.HasOne(s => s.Staff)
                .WithMany()
                .HasForeignKey(s => s.StaffId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(s => s.Team)
                .WithMany()
                .HasForeignKey(s => s.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
