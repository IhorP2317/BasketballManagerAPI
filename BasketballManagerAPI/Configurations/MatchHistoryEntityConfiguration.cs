using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class MatchHistoryEntityConfiguration:BaseEntityConfiguration<MatchHistory> {

        public override void Configure(EntityTypeBuilder<MatchHistory> builder)
        {
            base.Configure(builder);
            builder.Property(m => m.MatchEventId)
                .HasConversion<int>();
        }
    }
}
