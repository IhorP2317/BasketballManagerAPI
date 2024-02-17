using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class PlayerExperienceEntityConfiguration:BaseEntityConfiguration<PlayerExperience> {
        public override void Configure(EntityTypeBuilder<PlayerExperience> builder)
        {
            base.Configure(builder);
            builder.HasOne(p => p.Player)
                .WithMany(p => p.PlayerExperiences)
                .HasForeignKey(p => p.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.Team)
                .WithMany(p => p.PlayerExperiences)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
