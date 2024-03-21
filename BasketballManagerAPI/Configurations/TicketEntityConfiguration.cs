using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class TicketEntityConfiguration:BaseEntityConfiguration<Ticket> {
        public override void Configure(EntityTypeBuilder<Ticket> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.Price).HasColumnType("decimal(10,2)");
            builder.HasOne(t => t.Match)
                .WithMany(m => m.Tickets)
                .HasForeignKey(t => t.MatchId);

            builder.HasOne(t => t.Order)
                .WithMany(t => t.Tickets)
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
