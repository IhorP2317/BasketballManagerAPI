using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class TransactionEntityConfiguration:BaseEntityConfiguration<Transaction> {
        public override void Configure(EntityTypeBuilder<Transaction> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.Value).HasColumnType("decimal(10,2)");

            builder.HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
