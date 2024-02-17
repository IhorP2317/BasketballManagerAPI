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
        private readonly IMapper _mapper;

        public StatisticService(ApplicationDbContext context, IMapper mapper, IMatchService matchService, IPlayerService playerService) {
            _context = context;
            _mapper = mapper;
            _matchService = matchService;
            _playerService = playerService;
        }


        public async Task<IEnumerable<PlayerStatisticDto>> GetAllMatchStatisticAsync(Guid matchId,
    MatchStatisticFiltersDto matchStatisticFiltersDto, CancellationToken cancellationToken = default) {
            if (!await _matchService.IsMatchExist(matchId, cancellationToken))
                throw new NotFoundException($"Match with id {matchId} is not found!");
            IQueryable<Statistic> statisticQuery = _context.Statistics.AsNoTracking()
                .Include(s => s.Player)
                .Where(m => m.MatchId == matchId);
            if (matchStatisticFiltersDto.TeamId.HasValue) {
                statisticQuery = statisticQuery.Where(m => m.Player.TeamId == matchStatisticFiltersDto.TeamId);
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
                .GroupBy(s => s.PlayerId)
                .Select(group => {
                    var player = group.First().Player;
                    var courtTime = group.Sum(g => g.CourtTime.Ticks);

                    return new PlayerStatisticDto {
                        FullName = $"{player.FirstName} {player.LastName}",
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

        public async Task<IEnumerable<TotalAnnuallyStatisticDto>> GetAllAnnuallyStatisticAsync(Guid playerId, TotalStatisticFiltersDto statisticFiltersDto,
    CancellationToken cancellationToken = default) {
            if (!await _playerService.IsPlayerExistAsync(playerId, cancellationToken))
                throw new NotFoundException($"Player with id {playerId} is not found!");
            IQueryable<Statistic> statisticsQuery = _context.Statistics
                .AsNoTracking()
                .Include(s => s.Match)
                .Where(s => s.PlayerId == playerId);
            if (statisticFiltersDto.Year != null)
                statisticsQuery = statisticsQuery.Where(s => s.Match.StartTime.Year == statisticFiltersDto.Year);
            var statistics = await statisticsQuery.ToListAsync(cancellationToken);

            var annualStatistic = statistics.GroupBy(s => s.Match.StartTime.Year)
                .Select(group => new TotalAnnuallyStatisticDto {
                    Year = group.Key,
                    MatchCount = group.Select(g => g.MatchId).Distinct().Count(),
                    Points = group.Sum(s => s.OnePointShotHitCount + s.TwoPointShotHitCount * 2 + s.ThreePointShotHitCount * 3),
                    OnePtShotCount = group.Sum(s => s.OnePointShotHitCount),
                    OnePtShotMissCount = group.Sum(s => s.OnePointShotMissCount),
                    TwoPtShotCount = group.Sum(s => s.TwoPointShotHitCount),
                    TwoPtShotMissCount = group.Sum(s => s.TwoPointShotMissCount),
                    ThreePtShotCount = group.Sum(s => s.ThreePointShotHitCount),
                    ThreePtShotMissCount = group.Sum(s => s.ThreePointShotMissCount),
                    AssistCount = group.Sum(s => s.AssistCount),
                    OffensiveReboundCount = group.Sum(s => s.OffensiveReboundCount),
                    DefensiveReboundCount = group.Sum(s => s.DefensiveReboundCount),
                    StealCount = group.Sum(s => s.StealCount),
                    BlockCount = group.Sum(s => s.BlockCount),
                    TurnOverCount = group.Sum(s => s.TurnoverCount),
                    CourtTime = group.Aggregate(TimeSpan.Zero, (sum, next) => sum + next.CourtTime)
                })
                .OrderByDescending(s => s.Year)
                .ToList();
            return annualStatistic;
        }

        public async Task<StatisticDto> GetStatisticAsync(Guid playerId, Guid matchId, int timeUnit, CancellationToken cancellationToken = default) {
            var statistic = await _context.Statistics.AsNoTracking().FirstOrDefaultAsync(s => s.PlayerId == playerId
                && s.MatchId == matchId
                && s.TimeUnit == timeUnit, cancellationToken);
            if (statistic == null)
            {
                throw new NotFoundException(
                    $"Statistic with player Id {playerId}, with match id {matchId} and timeUnit {timeUnit} not found!");
            }
            return _mapper.Map<StatisticDto>(statistic);
        }
        public async Task<bool> IsStatisticExistAsync(Guid matchId, Guid playerId, int timeUnit, CancellationToken cancellationToken = default) {
            return await _context.Statistics.AnyAsync(s => s.MatchId == matchId && s.PlayerId == playerId && s.TimeUnit == timeUnit, cancellationToken);
        }

        public async Task<StatisticDto> CreateStatisticAsync(StatisticDto statisticDto, CancellationToken cancellationToken = default) {
            if (!await _matchService.IsMatchExist(statisticDto.MatchId, cancellationToken))
                throw new NotFoundException("The match of created statistic does not exist!");
            if (!await _playerService.IsPlayerExistAsync(statisticDto.PlayerId, cancellationToken))
                throw new NotFoundException("The player of created statistic does not exist!");
            if (await IsStatisticExistAsync(statisticDto.MatchId, statisticDto.PlayerId,
                    statisticDto.TimeUnit.GetValueOrDefault(), cancellationToken))
                throw new DomainException("Statistic ",
                    $"match id {statisticDto.MatchId}\n player id {statisticDto.PlayerId}\n timeUnit {statisticDto.TimeUnit}");
            var statistic = _mapper.Map<Statistic>(statisticDto);
            var createdStatistic = await _context.Statistics.AddAsync(statistic, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<StatisticDto>(createdStatistic.Entity);
        }

        public async Task UpdateStatisticAsync(StatisticDto statisticDto, CancellationToken cancellationToken = default) {
            var foundedStatistic = await _context.Statistics.FindAsync(new object[] { statisticDto.MatchId, statisticDto.PlayerId, statisticDto.TimeUnit.GetValueOrDefault() }, cancellationToken);
            if (foundedStatistic == null) {
                throw new NotFoundException($"Statistic with match id {statisticDto.MatchId}, player id {statisticDto.PlayerId} and time Unit {statisticDto.TimeUnit.GetValueOrDefault()} not found.");
            }

            _mapper.Map(statisticDto, foundedStatistic);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteStatisticAsync(Guid playerId, Guid matchId, int timeUnit,
            CancellationToken cancellationToken) {
            var foundedStatistic = await _context.Statistics.FindAsync(new object[] { matchId, playerId, timeUnit }, cancellationToken);
            if (foundedStatistic == null) {
                throw new NotFoundException($"Statistic with match id {matchId}, player id {playerId} and time Unit {timeUnit} not found.");
            }

            _context.Statistics.Remove(foundedStatistic);
            await _context.SaveChangesAsync(cancellationToken);
        }
        
    }
}
