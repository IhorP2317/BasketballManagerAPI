using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class PlayerEntityConfiguration:BaseEntityConfiguration<Player> {
        public override void Configure(EntityTypeBuilder<Player> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Height)
                .HasColumnType("decimal(3,1)");
            builder.Property(p => p.Weight)
                .HasColumnType("decimal(3,1)");
            builder.HasIndex(p => new { p.TeamId, p.JerseyNumber }).IsUnique();
            
            builder.HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
