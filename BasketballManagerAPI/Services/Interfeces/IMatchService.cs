using BasketballManagerAPI.Dto.CoachDto;
using BasketballManagerAPI.Dto.MatchDto;
using BasketballManagerAPI.Helpers;

namespace BasketballManagerAPI.Services.Interfeces {
    public interface IMatchService {
        Task<PagedList<MatchResponseDto>> GetAllMatchesAsync(MatchFiltersDto matchFiltersDto, CancellationToken  cancellationToken = default);
        Task<MatchResponseDto> GetMatchAsync(Guid id, CancellationToken cancellationToken = default);
        Task<MatchDetailDto> GetMatchDetailAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsMatchExist(Guid id, CancellationToken cancellationToken = default);
        Task<MatchResponseDto> CreateMatchAsync(MatchRequestDto matchDto, CancellationToken cancellationToken = default);
        Task UpdateMatchAsync(Guid id, MatchRequestDto matchDto, CancellationToken cancellationToken = default);
        Task DeleteMatchAsync(Guid id, CancellationToken cancellationToken = default);

    }
}
