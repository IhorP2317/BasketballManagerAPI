using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class StaffAwardEntityConfiguration: IEntityTypeConfiguration<StaffAward> {
        public virtual void Configure(EntityTypeBuilder<StaffAward> builder) {
            builder.HasKey(s => new { s.AwardId, s.StaffId });
            builder.HasOne(s => s.Staff)
                .WithMany()
                .HasForeignKey(s => s.StaffId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
