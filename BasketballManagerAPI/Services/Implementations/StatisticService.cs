using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.StatisticDto;
using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Helpers;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;

namespace BasketballManagerAPI.Services.Implementations {
    public class StatisticService : IStatisticService {
        private readonly ApplicationDbContext _context;
        private readonly IMatchService _matchService;
        private readonly IPlayerService _playerService;
        private readonly ITeamService _teamService;
        private readonly IPlayerExperienceService _playerExperienceService;
        private readonly IMapper _mapper;

        public StatisticService(ApplicationDbContext context, IMapper mapper, IMatchService matchService, IPlayerService playerService, ITeamService teamService, IPlayerExperienceService playerExperienceService) {
            _context = context;
            _mapper = mapper;
            _matchService = matchService;
            _playerService = playerService;
            _teamService = teamService;
            _playerExperienceService = playerExperienceService;
        }


        public async Task<IEnumerable<PlayerStatisticDto>> GetAllPlayersStatisticByMatchAsync(Guid matchId,
    MatchStatisticFiltersDto matchStatisticFiltersDto, CancellationToken cancellationToken = default) {
            if (!await _matchService.IsMatchExist(matchId, cancellationToken))
                throw new NotFoundException($"Match with id {matchId} is not found!");
            IQueryable<Statistic> statisticQuery = _context.Statistics.AsNoTracking()
                .Include(s => s.PlayerExperience)
                .Include(s => s.Match)
                .Where(s => s.MatchId == matchId);
            if (matchStatisticFiltersDto.TeamId.HasValue) {
                statisticQuery = statisticQuery.Where(s => s.PlayerExperience.TeamId == matchStatisticFiltersDto.TeamId);
            }

            if (matchStatisticFiltersDto.TimeUnit.HasValue) {
                if (matchStatisticFiltersDto.IsAccumulativeDisplayEnabled.GetValueOrDefault()) {
                    statisticQuery = statisticQuery
                        .Where(s => s.TimeUnit <= matchStatisticFiltersDto.TimeUnit.Value);
                } else {
                    statisticQuery = statisticQuery
                        .Where(s => s.TimeUnit == matchStatisticFiltersDto.TimeUnit.Value);
                }
            }


            var statistics = await statisticQuery.ToListAsync(cancellationToken);

            var playerStatistics = statistics
                .GroupBy(s => s.PlayerExperience.PlayerId)
                .Select(group => {
                    var player = group.First().PlayerExperience.Player;
                    var courtTime = group.Sum(g => g.CourtTime.Ticks);

                    return new PlayerStatisticDto {
                        FullName = $"{player.FirstName} {player.LastName}",
                        TeamId = player.TeamId,
                        Points = group.Sum(g => g.OnePointShotHitCount + (g.TwoPointShotHitCount * 2) + (g.ThreePointShotHitCount * 3)),
                        OnePointShotHit = group.Sum(g => g.OnePointShotHitCount),
                        OnePointShotMiss = group.Sum(g => g.OnePointShotMissCount),
                        TwoPointShotHit = group.Sum(g => g.TwoPointShotHitCount),
                        TwoPointShotMiss = group.Sum(g => g.TwoPointShotMissCount),
                        ThreePointShotHit = group.Sum(g => g.ThreePointShotHitCount),
                        ThreePointShotMiss = group.Sum(g => g.ThreePointShotMissCount),
                        Assists = group.Sum(g => g.AssistCount),
                        OffensiveRebounds = group.Sum(g => g.OffensiveReboundCount),
                        DefensiveRebounds = group.Sum(g => g.DefensiveReboundCount),
                        Steals = group.Sum(g => g.StealCount),
                        Blocks = group.Sum(g => g.BlockCount),
                        Turnovers = group.Sum(g => g.TurnoverCount),
                        CourtTime = new TimeSpan(courtTime)
                    };
                }).ToList();

            return playerStatistics;
        }

        public async Task<IEnumerable<MatchTeamStaticDto>> GetAllTeamsStatisticByMatchAsync(Guid matchId,
            MatchStatisticFiltersDto matchStatisticFiltersDto, CancellationToken cancellationToken = default) {
            var playerStatistics = await GetAllPlayersStatisticByMatchAsync(matchId, matchStatisticFiltersDto, cancellationToken);


            var teams = await _teamService.GetAllTeamsAsync(cancellationToken);

            var teamStatistics = playerStatistics
                .GroupBy(ps => ps.TeamId)
                .Select(group => {
                    var team = teams.FirstOrDefault(t => t.Id == group.Key);
                    var teamName = team?.Name ?? "Unknown Team";
                    var teamStats = group.Select(x => x).ToList();
                    return new MatchTeamStaticDto {
                        Name = teamName,
                        Statistics = teamStats
                    };
                });

            return teamStatistics;
        }

        public async Task<IEnumerable<TotalAnnuallyStatisticDto>> GetAllAnnuallyStatisticAsync(Guid playerId, TotalStatisticFiltersDto statisticFiltersDto,
    CancellationToken cancellationToken = default) {
            if (!await _playerService.IsPlayerExistAsync(playerId, cancellationToken))
                throw new NotFoundException($"Player with id {playerId} is not found!");
            IQueryable<Statistic> statisticsQuery = _context.Statistics
                .AsNoTracking()
                .Include(s => s.Match)
                .Include(s => s.PlayerExperience)
                .Where(s => s.PlayerExperience.PlayerId == playerId);
            if (statisticFiltersDto.Year != null)
                statisticsQuery = statisticsQuery.Where(s => s.Match.StartTime.Year == statisticFiltersDto.Year);
            var statistics = await statisticsQuery.ToListAsync(cancellationToken);

            var annualStatistics = statistics.GroupBy(s => s.Match.StartTime.Year)
      .Select(group => {
          var onePtShotCount = group.Sum(s => s.OnePointShotHitCount);
          var onePtShotAttempt = onePtShotCount + group.Sum(s => s.OnePointShotMissCount);
          var twoPtShotCount = group.Sum(s => s.TwoPointShotHitCount);
          var twoPtShotAttempt = twoPtShotCount + group.Sum(s => s.TwoPointShotMissCount);
          var threePtShotCount = group.Sum(s => s.ThreePointShotHitCount);
          var threePtShotAttempt = threePtShotCount + group.Sum(s => s.ThreePointShotMissCount);

          return new TotalAnnuallyStatisticDto {
              Year = group.Key,
              MatchCount = group.Select(g => g.MatchId).Distinct().Count(),
              Points = group.Sum(s => s.OnePointShotHitCount + s.TwoPointShotHitCount * 2 + s.ThreePointShotHitCount * 3),
              OnePtShotCount = onePtShotCount,
              OnePtShotMissCount = group.Sum(s => s.OnePointShotMissCount),
              TwoPtShotCount = twoPtShotCount,
              TwoPtShotMissCount = group.Sum(s => s.TwoPointShotMissCount),
              ThreePtShotCount = threePtShotCount,
              ThreePtShotMissCount = group.Sum(s => s.ThreePointShotMissCount),
              AssistCount = group.Sum(s => s.AssistCount),
              OffensiveReboundCount = group.Sum(s => s.OffensiveReboundCount),
              DefensiveReboundCount = group.Sum(s => s.DefensiveReboundCount),
              StealCount = group.Sum(s => s.StealCount),
              BlockCount = group.Sum(s => s.BlockCount),
              TurnOverCount = group.Sum(s => s.TurnoverCount),
              CourtTime = group.Aggregate(TimeSpan.Zero, (sum, next) => sum + next.CourtTime),
              OnePointShotPercentage = onePtShotAttempt > 0 ? (double)onePtShotCount / onePtShotAttempt * 100 : 0,
              TwoPointShotPercentage = twoPtShotAttempt > 0 ? (double)twoPtShotCount / twoPtShotAttempt * 100 : 0,
              ThreePointShotPercentage = threePtShotAttempt > 0 ? (double)threePtShotCount / threePtShotAttempt * 100 : 0,
          };
      })
      .OrderByDescending(s => s.Year)
      .ToList();
            if (annualStatistics.Any()) {
                var totalStatistics = CalculateTotalStatistics(annualStatistics);
                var averageStatistics = CalculateAverageStatistics(annualStatistics);


                annualStatistics.Add(totalStatistics);
                annualStatistics.Add(averageStatistics);
            }


            return annualStatistics;
        }
        private TotalAnnuallyStatisticDto CalculateTotalStatistics(List<TotalAnnuallyStatisticDto> annualStatistics) {
            var totalStatistics = new TotalAnnuallyStatisticDto {
                Year = 0,
                MatchCount = annualStatistics.Sum(s => s.MatchCount),
                Points = annualStatistics.Sum(s => s.Points),
                OnePtShotCount = annualStatistics.Sum(s => s.OnePtShotCount),
                TwoPtShotCount = annualStatistics.Sum(s => s.TwoPtShotCount),
                ThreePtShotCount = annualStatistics.Sum(s => s.ThreePtShotCount),
                OnePtShotMissCount = annualStatistics.Sum(s => s.OnePtShotMissCount),
                TwoPtShotMissCount = annualStatistics.Sum(s => s.TwoPtShotMissCount),
                ThreePtShotMissCount = annualStatistics.Sum(s => s.ThreePtShotMissCount),
                AssistCount = annualStatistics.Sum(s => s.AssistCount),
                OffensiveReboundCount = annualStatistics.Sum(s => s.OffensiveReboundCount),
                DefensiveReboundCount = annualStatistics.Sum(s => s.DefensiveReboundCount),
                StealCount = annualStatistics.Sum(s => s.StealCount),
                BlockCount = annualStatistics.Sum(s => s.BlockCount),
                TurnOverCount = annualStatistics.Sum(s => s.TurnOverCount),
                CourtTime = TimeSpan.FromTicks(annualStatistics.Sum(s => s.CourtTime.Ticks)),
            };


            totalStatistics.OnePointShotPercentage = CalculatePercentage(totalStatistics.OnePtShotCount, totalStatistics.OnePtShotCount + totalStatistics.OnePtShotMissCount);
            totalStatistics.TwoPointShotPercentage = CalculatePercentage(totalStatistics.TwoPtShotCount, totalStatistics.TwoPtShotCount + totalStatistics.TwoPtShotMissCount);
            totalStatistics.ThreePointShotPercentage = CalculatePercentage(totalStatistics.ThreePtShotCount, totalStatistics.ThreePtShotCount + totalStatistics.ThreePtShotMissCount);

            return totalStatistics;
        }
        private TotalAnnuallyStatisticDto CalculateAverageStatistics(List<TotalAnnuallyStatisticDto> annualStatistics) {
            var annualCount = annualStatistics.Count;

            var averageStatistics = new TotalAnnuallyStatisticDto {
                Year = -1,
                MatchCount = annualStatistics.Average(s => s.MatchCount),
                Points = annualStatistics.Average(s => s.Points),
                OnePtShotCount = annualStatistics.Average(s => s.OnePtShotCount),
                TwoPtShotCount = annualStatistics.Average(s => s.TwoPtShotCount),
                ThreePtShotCount = annualStatistics.Average(s => s.ThreePtShotCount),
                OnePtShotMissCount = annualStatistics.Average(s => s.OnePtShotMissCount),
                TwoPtShotMissCount = annualStatistics.Average(s => s.TwoPtShotMissCount),
                ThreePtShotMissCount = annualStatistics.Average(s => s.ThreePtShotMissCount),
                AssistCount = annualStatistics.Average(s => s.AssistCount),
                OffensiveReboundCount = annualStatistics.Average(s => s.OffensiveReboundCount),
                DefensiveReboundCount = annualStatistics.Average(s => s.DefensiveReboundCount),
                StealCount = annualStatistics.Average(s => s.StealCount),
                BlockCount = annualStatistics.Average(s => s.BlockCount),
                TurnOverCount = annualStatistics.Average(s => s.TurnOverCount),

                CourtTime = TimeSpan.FromTicks((long)annualStatistics.Average(s => s.CourtTime.Ticks)),
            };


            averageStatistics.OnePointShotPercentage = annualStatistics.Sum(s => s.OnePointShotPercentage) / annualCount;
            averageStatistics.TwoPointShotPercentage = annualStatistics.Sum(s => s.TwoPointShotPercentage) / annualCount;
            averageStatistics.ThreePointShotPercentage = annualStatistics.Sum(s => s.ThreePointShotPercentage) / annualCount;

            return averageStatistics;
        }

        private double CalculatePercentage(double hits, double attempts) {
            return attempts > 0 ? (hits / attempts) * 100 : 0;
        }

        public async Task<IEnumerable<TotalTeamStatisticDto>> GetAllTotalTeamsStatisticByMatchAsync(Guid matchId,
            CancellationToken cancellationToken)
        {
            if (!await _matchService.IsMatchExist(matchId, cancellationToken))
                throw new NotFoundException($"Match with id {matchId} is not found!");
            IQueryable<Statistic> statisticsQuery = _context.Statistics
                .AsNoTracking()
                .Include(s => s.Match)
                .Include(s => s.PlayerExperience)
                .ThenInclude(p => p.Team)
                .Where(s => s.MatchId == matchId);
            var statisticsData = await statisticsQuery.Select(s => new
            {
                TeamName = s.PlayerExperience.Team.Name,
                s.OnePointShotHitCount,
                s.OnePointShotMissCount,
                s.TwoPointShotHitCount,
                s.TwoPointShotMissCount,
                s.ThreePointShotHitCount,
                s.ThreePointShotMissCount,
                s.AssistCount,
                s.OffensiveReboundCount,
                s.DefensiveReboundCount,
                s.StealCount,
                s.BlockCount,
                s.TurnoverCount
            }).ToListAsync(cancellationToken);

            var totalTeamStatistics = statisticsData
                .GroupBy(s => s.TeamName)
                .Select(group => {
                    var onePtShotCount = group.Sum(s => s.OnePointShotHitCount);
                    var onePtShotAttempt = onePtShotCount + group.Sum(s => s.OnePointShotMissCount);
                    var twoPtShotCount = group.Sum(s => s.TwoPointShotHitCount);
                    var twoPtShotAttempt = twoPtShotCount + group.Sum(s => s.TwoPointShotMissCount);
                    var threePtShotCount = group.Sum(s => s.ThreePointShotHitCount);
                    var threePtShotAttempt = threePtShotCount + group.Sum(s => s.ThreePointShotMissCount);

                    return new TotalTeamStatisticDto {
                        Name = group.Key,
                        Points = onePtShotCount + (twoPtShotCount * 2) + (threePtShotCount * 3),
                        OnePointShotsCompleted = onePtShotCount,
                        OnePointShotsMissed = group.Sum(s => s.OnePointShotMissCount),
                        TwoPointShotsCompleted = twoPtShotCount,
                        TwoPointShotsMissed = group.Sum(s => s.TwoPointShotMissCount),
                        ThreePointShotsCompleted = threePtShotCount,
                        ThreePointShotsMissed = group.Sum(s => s.ThreePointShotMissCount),
                        Assists = group.Sum(s => s.AssistCount),
                        OffensiveRebounds = group.Sum(s => s.OffensiveReboundCount),
                        DefensiveRebounds = group.Sum(s => s.DefensiveReboundCount),
                        Steals = group.Sum(s => s.StealCount),
                        Blocks = group.Sum(s => s.BlockCount),
                        TurnOvers = group.Sum(s => s.TurnoverCount),
                        OnePointShotPercentage = onePtShotAttempt > 0 ? (double)onePtShotCount / onePtShotAttempt * 100 : 0,
                        TwoPointShotPercentage = twoPtShotAttempt > 0 ? (double)twoPtShotCount / twoPtShotAttempt * 100 : 0,
                        ThreePointShotPercentage = threePtShotAttempt > 0 ? (double)threePtShotCount / threePtShotAttempt * 100 : 0,
                    };
                }).ToList();
            return totalTeamStatistics;
        }


        public async Task<IEnumerable<PlayerImpactStatisticDto>> CalculatePlayerImpactInMatchAsync(Guid matchId, CancellationToken cancellationToken) {
           
            var totalTeamStatistics = await GetAllTotalTeamsStatisticByMatchAsync(matchId, cancellationToken);

         
            var playerStatistics = await GetAllPlayersStatisticByMatchAsync(matchId, new MatchStatisticFiltersDto(), cancellationToken);

            List<PlayerImpactStatisticDto> playerImpactStatistics = new List<PlayerImpactStatisticDto>();

            foreach (var playerStat in playerStatistics) {

                var team = await _teamService.GetTeamAsync(playerStat.TeamId.GetValueOrDefault(), cancellationToken);
                if (team == null)
                    throw new NotFoundException(
                        $"Team with id {playerStat.TeamId.GetValueOrDefault()} does not exist!");
                var teamTotalStat = totalTeamStatistics.FirstOrDefault(t => t.Name == team.Name);
                if (teamTotalStat == null) continue; 

                var impact = new PlayerImpactStatisticDto {
                    TeamId = playerStat.TeamId.GetValueOrDefault(),
                    FullName = playerStat.FullName,
                    OnePointShotMakeShare = CalculatePercentage(playerStat.OnePointShotHit, teamTotalStat.OnePointShotsCompleted),
                    OnePointShotMissShare = CalculatePercentage(playerStat.OnePointShotMiss, teamTotalStat.OnePointShotsMissed),
                    TwoPointShotMakeShare = CalculatePercentage(playerStat.TwoPointShotHit, teamTotalStat.TwoPointShotsCompleted),
                    TwoPointShotMissShare = CalculatePercentage(playerStat.TwoPointShotMiss, teamTotalStat.TwoPointShotsMissed),
                    ThreePointShotMakeShare = CalculatePercentage(playerStat.ThreePointShotHit, teamTotalStat.ThreePointShotsCompleted),
                    ThreePointShotMissShare = CalculatePercentage(playerStat.ThreePointShotMiss, teamTotalStat.ThreePointShotsMissed),
                    PointsShare = CalculatePercentage(playerStat.Points, teamTotalStat.Points),
                    AssistsShare = CalculatePercentage(playerStat.Assists, teamTotalStat.Assists),
                    OffensiveReboundsShare = CalculatePercentage(playerStat.OffensiveRebounds, teamTotalStat.OffensiveRebounds),
                    DefensiveReboundsShare = CalculatePercentage(playerStat.DefensiveRebounds, teamTotalStat.DefensiveRebounds),
                    StealsShare = CalculatePercentage(playerStat.Steals, teamTotalStat.Steals),
                    BlocksShare = CalculatePercentage(playerStat.Blocks, teamTotalStat.Blocks),
                    TurnoversShare = CalculatePercentage(playerStat.Turnovers, teamTotalStat.TurnOvers)
                };

                playerImpactStatistics.Add(impact);
            }

            return playerImpactStatistics;
        }

        public async Task<StatisticDto> GetStatisticAsync(Guid playerExperienceId, Guid matchId, int timeUnit, CancellationToken cancellationToken = default) {
            var statistic = await _context.Statistics.AsNoTracking().FirstOrDefaultAsync(s => s.PlayerExperienceId == playerExperienceId
                && s.MatchId == matchId
                && s.TimeUnit == timeUnit, cancellationToken);
            if (statistic == null) {
                throw new NotFoundException(
                    $"Statistic with player Id {playerExperienceId}, with match id {matchId} and timeUnit {timeUnit} not found!");
            }
            return _mapper.Map<StatisticDto>(statistic);
        }
        public async Task<bool> IsStatisticExistAsync(Guid matchId, Guid playerExperienceId, int timeUnit, CancellationToken cancellationToken = default) {
            return await _context.Statistics.AnyAsync(s => s.MatchId == matchId && s.PlayerExperienceId == playerExperienceId && s.TimeUnit == timeUnit, cancellationToken);
        }

        public async Task<StatisticDto> CreateStatisticAsync(StatisticDto statisticDto, CancellationToken cancellationToken = default)
        {
            var match = await _context.Matches.FindAsync(statisticDto.MatchId, cancellationToken);
            if(match == null)
                throw new NotFoundException("The match of created statistic does not exist!");
            var playerExperience =
                await _context.PlayerExperiences.FindAsync(statisticDto.PlayerExperienceId, cancellationToken);
            if (playerExperience == null)
                throw new NotFoundException("The player experience of created statistic does not exist!");
            if (await IsStatisticExistAsync(statisticDto.MatchId, statisticDto.PlayerExperienceId,
                    statisticDto.TimeUnit.GetValueOrDefault(), cancellationToken))
                throw new DomainUniquenessException("Statistic ",
                    $"match id {statisticDto.MatchId}\n player id {statisticDto.PlayerExperienceId}\n timeUnit {statisticDto.TimeUnit}");
            if (playerExperience.TeamId != match.AwayTeamId && playerExperience.TeamId != match.HomeTeamId)
                throw new DomainLogicException(
                    $"Can not create Statistic to player that did not be a part of teams, that had played  match with id {match.Id}!");
            var playerExperienceStartDate = new DateTime(playerExperience.StartDate.Year, playerExperience.StartDate.Month, playerExperience.StartDate.Day);

            DateTime? playerExperienceEndDate = null;
            if (playerExperience.EndDate.HasValue) {
                playerExperienceEndDate = new DateTime(playerExperience.EndDate.Value.Year, playerExperience.EndDate.Value.Month, playerExperience.EndDate.Value.Day);
            }

          
            if (match.StartTime < playerExperienceStartDate || (playerExperienceEndDate.HasValue && match.StartTime > playerExperienceEndDate.Value)) {
                throw new DomainLogicException($"Match start time {match.StartTime} is outside the player experience range from {playerExperienceStartDate} to {playerExperienceEndDate?.ToString() ?? "now"}.");
            }

            var statistic = _mapper.Map<Statistic>(statisticDto);
            var createdStatistic = await _context.Statistics.AddAsync(statistic, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<StatisticDto>(createdStatistic.Entity);
        }

        public async Task UpdateStatisticAsync(StatisticDto statisticDto, CancellationToken cancellationToken = default) {
            var foundedStatistic = await _context.Statistics.FindAsync(new object[] { statisticDto.MatchId, statisticDto.PlayerExperienceId, statisticDto.TimeUnit.GetValueOrDefault() }, cancellationToken);
            if (foundedStatistic == null) {
                throw new NotFoundException($"Statistic with match id {statisticDto.MatchId}, player id {statisticDto.PlayerExperienceId} and time Unit {statisticDto.TimeUnit.GetValueOrDefault()} not found.");
            }

            _mapper.Map(statisticDto, foundedStatistic);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteStatisticAsync(Guid playerExperienceId, Guid matchId, int timeUnit,
            CancellationToken cancellationToken) {
            var foundedStatistic = await _context.Statistics.FindAsync(new object[] { matchId, playerExperienceId, timeUnit }, cancellationToken);
            if (foundedStatistic == null) {
                throw new NotFoundException($"Statistic with match id {matchId}, player id {playerExperienceId} and time Unit {timeUnit} not found.");
            }

            _context.Statistics.Remove(foundedStatistic);
            await _context.SaveChangesAsync(cancellationToken);
        }


    }
}
