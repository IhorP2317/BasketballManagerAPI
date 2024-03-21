using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class OrderEntityConfiguration:BaseEntityConfiguration<Order> {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.TotalPrice).HasColumnType("decimal(10,2)");

            builder.HasOne(t => t.User)
                .WithMany(u => u.Orders)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
