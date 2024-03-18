using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class PlayerAwardEntityConfiguration:IEntityTypeConfiguration<PlayerAward> {
        public virtual void Configure(EntityTypeBuilder<PlayerAward> builder)
        {
            builder.HasKey(p => new { p.AwardId, p.PlayerExperienceId });
            builder.HasOne(p => p.PlayerExperience)
                .WithMany(p => p.PlayerAwards)
                .HasForeignKey(p => p.PlayerExperienceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
