using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasketballManagerAPI.Configurations {
    public class TeamEntityConfiguration:BaseEntityConfiguration<Team> {
        public override void Configure(EntityTypeBuilder<Team> builder)
        {
            base.Configure(builder);
            builder.HasIndex(t => t.Name).IsUnique();
        }
    }
}
