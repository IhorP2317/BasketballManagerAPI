using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Helpers;
using BasketballManagerAPI.Models;
using Security.Dto;

namespace BasketballManagerAPI.Services.Interfeces {
    public interface IPlayerService
    {

        Task<PagedList<PlayerResponseDto>> GetAllPlayersAsync(PlayerFiltersDto playerFiltersDto, CancellationToken cancellationToken = default);
        Task<PlayerResponseDto> GetPlayerByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PlayerDetailDto> GetPlayerByIdDetailAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsPlayerExistAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PlayerDetailDto> CreatePlayerAsync(PlayerRequestDto playerDto, CancellationToken cancellationToken = default);
        Task UpdatePlayerAsync(Guid id, PlayerUpdateDto playerDto, CancellationToken cancellationToken = default);
        Task UpdatePlayerTeamAsync(Guid playerId, Guid? newTeamId, CancellationToken cancellationToken = default);
        Task UpdatePlayerAvatarAsync(Guid id, IFormFile picture, CancellationToken cancellationToken = default);
        Task<FileDto> DownloadPlayerAvatarAsync(Guid id, CancellationToken cancellationToken = default);
        Task DeletePlayerAsync(Guid id, CancellationToken cancellationToken = default);
        
    }
}
