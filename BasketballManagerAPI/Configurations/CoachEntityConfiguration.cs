using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class CoachEntityConfiguration:BaseEntityConfiguration<Coach> {
        public override void Configure(EntityTypeBuilder<Coach> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.Specialty)
                .HasConversion(
                    s => s.ToString(),
                    s => (Specialty)Enum.Parse(typeof(Specialty), s));

            builder.Property(c => c.CoachStatusId)
                .HasConversion<int>();

            

        }
    }
}
