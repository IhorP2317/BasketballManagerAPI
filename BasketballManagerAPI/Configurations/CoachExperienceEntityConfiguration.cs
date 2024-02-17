using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class CoachExperienceEntityConfiguration:BaseEntityConfiguration<CoachExperience> {
        public override void Configure(EntityTypeBuilder<CoachExperience> builder)
        {
            base.Configure(builder);
            builder.HasOne(c => c.Coach)
                .WithMany(c => c.CoachExperiences)
                .HasForeignKey(c => c.CoachId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(c => c.Team)
                .WithMany(c => c.CoachExperiences)
                .HasForeignKey(c => c.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
