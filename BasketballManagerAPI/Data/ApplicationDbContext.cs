using BasketballManagerAPI.Configurations;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfeces;
using BasketballManagerAPI.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
        public DbSet<Order> Orders { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        private readonly ICurrentUserService _currentUserService;
        private readonly SuperAdminSeedData _superAdminSeedData;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService, IOptions<SuperAdminSeedData> superAdminSeedData) : base(options) {
            _currentUserService = currentUserService;
            _superAdminSeedData = superAdminSeedData.Value;

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
            modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TeamEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TicketEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SuperAdminEntityConfiguration(_superAdminSeedData));

        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

           return await SaveAllChangesAsync(cancellationToken);
        }

        private async Task<int> SaveAllChangesAsync(CancellationToken cancellationToken)
        {
            OnBeforeSaving();

            var matchesToRecalculate = DetectMatchesToRecalculate();
            var saveResult = await base.SaveChangesAsync(cancellationToken);

            await RecalculateMatchScoresAsync(matchesToRecalculate, cancellationToken);


            if (ChangeTracker.HasChanges()) {
                saveResult += await base.SaveChangesAsync(cancellationToken);
            }

            return saveResult;
        }
        private void OnBeforeSaving() {
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries) {
                if (entry.Entity is BaseEntity baseEntity) {
                    switch (entry.State) {
                        case EntityState.Modified:
                            baseEntity.ModifiedTime = DateTime.Now;
                            baseEntity.ModifiedById = Guid.Parse(_currentUserService.UserId);
                            break;

                        case EntityState.Added:
                            baseEntity.CreatedTime = DateTime.Now;
                            if (_currentUserService.UserId != null)
                                baseEntity.CreatedById = Guid.Parse(_currentUserService.UserId);
                            if (baseEntity.Id == default)
                                baseEntity.Id = Guid.NewGuid();
                            break;
                    }
                }
            }
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
                    .ThenInclude(s => s.PlayerExperience)
                    .ThenInclude(p => p.Player)
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
                .Where(s => s.PlayerExperience.TeamId == match.HomeTeamId)
                .Sum(s => CalculateScore(s));

            match.AwayTeamScore = match.Statistics
                .Where(s => s.PlayerExperience.TeamId == match.AwayTeamId)
                .Sum(s => CalculateScore(s));
        }

        private int CalculateScore(Statistic statistic) {
            return statistic.OnePointShotHitCount +
                   statistic.TwoPointShotHitCount * 2 +
                   statistic.ThreePointShotHitCount * 3;
        }

    }
}
