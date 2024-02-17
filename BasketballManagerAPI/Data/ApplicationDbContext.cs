using BasketballManagerAPI.Configurations;
using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BasketballManagerAPI.Data {
    public class ApplicationDbContext:DbContext {
        public DbSet<Award> Awards { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<CoachStatus> CoachesStatuses { get; set; }
        public DbSet<Match> Matches { get; set; }  
        public DbSet<MatchEvent> MatchEvents { get; set; }
        public DbSet<MatchHistory> MatchHistories { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<StaffExperience> StaffExperiences { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<StaffAward> StaffAwards { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
            
        }
        public ApplicationDbContext() {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AwardEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CoachEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CoachStatusEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerEntityConfiguration());
            modelBuilder.Entity<StaffAward>()
                .HasKey(s => new { s.AwardId, s.StaffId });
        }

       
    }
}
