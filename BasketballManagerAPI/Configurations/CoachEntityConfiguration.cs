using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class CoachEntityConfiguration:BaseEntityConfiguration<Coach> {
        public override void Configure(EntityTypeBuilder<Coach> builder)
        {
            base.Configure(builder);
            builder.HasOne(c => c.Team)
                .WithMany( t => t.Coaches)
                .HasForeignKey(c => c.TeamId)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
