using BasketballManagerAPI.Dto.StatisticDto;
using BasketballManagerAPI.Helpers;

namespace BasketballManagerAPI.Services.Interfeces
{
    public interface IStatisticService
    {
        Task<IEnumerable<PlayerStatisticDto>> GetAllPlayersStatisticByMatchAsync(Guid matchId,
            MatchStatisticFiltersDto matchStatisticFiltersDto, CancellationToken cancellationToken = default);

        Task<IEnumerable<MatchTeamStatisticDto>> GetAllTeamsStatisticByMatchAsync(Guid matchId,
            MatchStatisticFiltersDto matchStatisticFiltersDto, CancellationToken cancellationToken = default);

        Task<IEnumerable<TotalTeamStatisticDto>> GetAllTotalTeamsStatisticByMatchAsync(Guid matchId, MatchStatisticFiltersDto matchStatisticFiltersDto,
            CancellationToken cancellationToken = default);

       public Task<IEnumerable<TotalAnnuallyStatisticDto>> GetAllAnnuallyStatisticAsync(Guid playerId,
           TotalStatisticFiltersDto statisticFiltersDto,
           CancellationToken cancellationToken = default);

       Task<IEnumerable<PlayerImpactStatisticDto>> CalculatePlayerImpactInMatchAsync(Guid matchId,
           MatchStatisticFiltersDto matchStatisticFiltersDto,
           CancellationToken cancellationToken = default);

       public Task<StatisticDto> GetStatisticAsync(Guid playerId, Guid matchId, int timeUnit,
           CancellationToken cancellationToken = default);

       public  Task<bool> IsStatisticExistAsync(Guid matchId, Guid playerId, int timeUnit,
           CancellationToken cancellationToken = default);

       public Task<StatisticDto> CreateStatisticAsync(StatisticDto statisticDto,
           CancellationToken cancellationToken = default);

       public Task UpdateStatisticAsync(StatisticDto statisticDto, CancellationToken cancellationToken = default);

       public  Task DeleteStatisticAsync(Guid playerExperienceId, Guid matchId, int timeUnit,
           CancellationToken cancellationToken);
    }
}
