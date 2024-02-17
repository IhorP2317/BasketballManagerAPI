using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class CoachAwardEntityConfiguration:IEntityTypeConfiguration<CoachAward> {
        public virtual void Configure(EntityTypeBuilder<CoachAward> builder)
        {
            builder.HasKey(c => new { c.AwardId, c.CoachId });
            builder.HasOne(c => c.Coach)
                .WithMany(c => c.CoachAwards)
                .HasForeignKey(c => c.CoachId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
