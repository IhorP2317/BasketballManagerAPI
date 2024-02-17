using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Helpers;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Services.Interfeces {
    public interface IPlayerService
    {

        Task<PagedList<PlayerResponseDto>> GetAllPlayersAsync(PlayerFiltersDto playerFiltersDto, CancellationToken cancellationToken = default);
        Task<PlayerResponseDto> GetPlayerByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PlayerDetailDto> GetPlayerByIdDetailAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsPlayerExistAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PlayerResponseDto> CreatePlayerAsync(PlayerRequestDto playerDto, CancellationToken cancellationToken = default);
        Task UpdatePlayerAsync(Guid id,PlayerRequestDto playerDto, CancellationToken cancellationToken = default);
        Task DeletePlayerAsync(Guid id, CancellationToken cancellationToken = default);
        
    }
}
