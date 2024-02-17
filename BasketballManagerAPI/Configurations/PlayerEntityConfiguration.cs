using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class PlayerEntityConfiguration:BaseEntityConfiguration<Player> {
        public override void Configure(EntityTypeBuilder<Player> builder)
        {
            base.Configure(builder);
            builder.HasMany(p => p.MatchHistories)
                .WithOne(mh => mh.Player)
                .HasForeignKey(mh => mh.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(p => p.Position)
                .HasConversion(
                    p => p.ToString(),
                    p => (Position)Enum.Parse(typeof(Position), p));


        }
    }
}
