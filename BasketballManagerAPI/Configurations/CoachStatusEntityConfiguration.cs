using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class CoachStatusEntityConfiguration: IEntityTypeConfiguration<CoachStatus> {
        public  virtual void Configure(EntityTypeBuilder<CoachStatus> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasConversion<int>();
            builder.HasData(
                Enum.GetValues(typeof(CoachStatusId))
                    .Cast<CoachStatusId>()
                    .Select(c => new CoachStatus()
                    {
                        Id = c,
                        Name = c.ToString()
                    })
                );

        }
    }
}
