using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class StatisticEntityConfiguration:IEntityTypeConfiguration<Statistic> {
        public virtual void Configure(EntityTypeBuilder<Statistic> builder)
        {
            builder.HasKey(s => new { s.MatchId, s.PlayerId, s.TimeUnit });
            builder.HasOne(s => s.Match)
                .WithMany(m => m.Statistics)
                .HasForeignKey(s => s.MatchId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(s => s.Player)
                .WithMany(p => p.Statistics)
                .HasForeignKey(s => s.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
