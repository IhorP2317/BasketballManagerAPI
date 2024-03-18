using BasketballManagerAPI.Dto.ExperienceDto;
using BasketballManagerAPI.Helpers;


namespace BasketballManagerAPI.Services.Interfeces {
    public interface IPlayerExperienceService
    {
        Task<PagedList<PlayerExperienceResponseDto>> GetAllPlayerExperiencesAsync(PlayerExperienceFiltersDto playerExperienceFiltersDto,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<PlayerExperienceResponseDto>> GetAllPlayerExperiencesByPlayerIdAsync(Guid playerId,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<PlayerExperienceDetailDto>> GetAllPlayerExperiencesByPlayerIdDetailAsync(Guid playerId,
            CancellationToken cancellationToken = default);
        Task<PlayerExperienceDetailDto> GetPlayerExperienceDetailAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<bool> IsPlayerExperienceExistAsync(Guid id,
            CancellationToken cancellationToken = default);
        Task<PlayerExperienceResponseDto> CreateExperienceAsync(Guid playerId, PlayerExperienceRequestDto playerExperienceRequestDto, CancellationToken cancellationToken = default);
        Task UpdatePlayerExperienceAsync(Guid id,
            PlayerExperienceUpdateDto playerExperienceUpdateDto, CancellationToken cancellationToken = default);
        Task DeletePlayerExperienceAsync(Guid id,CancellationToken  cancellationToken = default);
    }
}
