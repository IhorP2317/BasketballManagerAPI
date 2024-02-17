using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class AwardEntityConfiguration: BaseEntityConfiguration<Award> {
        public override void Configure(EntityTypeBuilder<Award> builder)
        {
            base.Configure(builder);
            builder.HasIndex(a => new { a.Name, a.Date }).IsUnique();
            builder.HasMany(a => a.PlayerAwards)
                .WithOne(p => p.Award)
                .HasForeignKey(p => p.AwardId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.CoachAwards)
                .WithOne(c => c.Award)
                .HasForeignKey(c => c.AwardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
