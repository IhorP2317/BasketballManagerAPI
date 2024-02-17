using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class MatchEntityConfiguration:BaseEntityConfiguration<Match> {
        public override void Configure(EntityTypeBuilder<Match> builder)
        {
            base.Configure(builder);
            builder.HasOne(m => m.HomeTeam)
                .WithMany()
                .HasForeignKey(m => m.HomeTeamId);
            builder.HasOne(m => m.AwayTeam)
                .WithMany()
                .HasForeignKey(m => m.AwayTeamId);

            builder.HasMany(m => m.MatchHistories)
                .WithOne(m => m.Match)
                .HasForeignKey(m => m.MatchId)
                .OnDelete(DeleteBehavior.Cascade); ;
        }
    }
}
