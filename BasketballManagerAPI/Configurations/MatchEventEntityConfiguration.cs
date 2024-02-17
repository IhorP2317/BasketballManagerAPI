using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class MatchEventEntityConfiguration:IEntityTypeConfiguration<MatchEvent> {
        public virtual void Configure(EntityTypeBuilder<MatchEvent> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).HasConversion<int>();
            builder.HasData(
                Enum.GetValues(typeof(MatchEventId))
                    .Cast<MatchEventId>()
                    .Select(m => new MatchEvent() {
                        Id = m,
                        Name = m.ToString()
                    })
            );
        }
    }
}
