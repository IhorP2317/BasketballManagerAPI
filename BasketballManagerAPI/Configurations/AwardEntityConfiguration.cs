using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class AwardEntityConfiguration: BaseEntityConfiguration<Award> {
        public override void Configure(EntityTypeBuilder<Award> builder)
        {
            base.Configure(builder);
            builder.HasMany(a => a.StaffAwards)
                .WithOne(s => s.Award)
                .HasForeignKey(s => s.AwardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
