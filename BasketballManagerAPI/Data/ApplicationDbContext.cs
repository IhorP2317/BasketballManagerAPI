using BasketballManagerAPI.Configurations;
using BasketballManagerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BasketballManagerAPI.Data {
    public class ApplicationDbContext:DbContext {
        public DbSet<Award> Awards { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Match> Matches { get; set; }  
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerExperience> PlayerExperiences { get; set; }
        public DbSet<CoachExperience> CoachExperiences { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<PlayerAward> PlayerAwards { get; set; }
        public DbSet<CoachAward> CoachAwards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Ticket> Tickets { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
            
        }
        public ApplicationDbContext() {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AwardEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CoachAwardEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CoachEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CoachExperienceEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerAwardEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerExperienceEntityConfiguration());
            modelBuilder.ApplyConfiguration(new MatchEntityConfiguration());
            modelBuilder.ApplyConfiguration(new StatisticEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TeamEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TicketEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());

        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

           return await SaveAllChangesAsync(cancellationToken);
        }

        private async Task<int> SaveAllChangesAsync(CancellationToken cancellationToken) {
   
            var matchesToRecalculate = DetectMatchesToRecalculate();

        
            var saveResult = await base.SaveChangesAsync(cancellationToken);

            await RecalculateMatchScoresAsync(matchesToRecalculate, cancellationToken);


            if (ChangeTracker.HasChanges()) {
                saveResult += await base.SaveChangesAsync(cancellationToken);
            }

            return saveResult;
        }

        private HashSet<Guid> DetectMatchesToRecalculate() {
            return new HashSet<Guid>(
                ChangeTracker
                    .Entries<Statistic>()
                    .Where(e => e.State == EntityState.Added ||
                                e.State == EntityState.Modified ||
                                e.State == EntityState.Deleted)
                    .Select(e => e.Entity.MatchId)
            );
        }

        private async Task<int> RecalculateMatchScoresAsync(HashSet<Guid> matchesToRecalculate, CancellationToken cancellationToken) {
            foreach (var matchId in matchesToRecalculate) {
                var match = await Matches
                    .Include(m => m.Statistics)
                    .ThenInclude(s => s.Player)
                    .SingleOrDefaultAsync(m => m.Id == matchId, cancellationToken);

                if (match != null) {
                    RecalculateMatchScores(match);
                }
            }

           
            if (ChangeTracker.HasChanges()) {
                return await base.SaveChangesAsync(cancellationToken);
            }

            return 0;
        }

        private void RecalculateMatchScores(Match match) {

            match.HomeTeamScore = match.Statistics
                .Where(s => s.Player.TeamId == match.HomeTeamId)
                .Sum(s => CalculateScore(s));

            match.AwayTeamScore = match.Statistics
                .Where(s => s.Player.TeamId == match.AwayTeamId)
                .Sum(s => CalculateScore(s));
        }

        private int CalculateScore(Statistic statistic) {
            return statistic.OnePointShotHitCount +
                   statistic.TwoPointShotHitCount * 2 +
                   statistic.ThreePointShotHitCount * 3;
        }

    }
}
