using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class TeamEntityConfiguration:BaseEntityConfiguration<Team> {
        public override void Configure(EntityTypeBuilder<Team> builder)
        {
            base.Configure(builder);
            builder.HasOne(t => t.Coach)
                .WithOne()
                .HasForeignKey<Team>(t => t.CoachId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
