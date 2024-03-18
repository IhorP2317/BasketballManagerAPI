using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Dto.CoachDto;
using Security.Dto;

namespace BasketballManagerAPI.Services.Interfeces {
    public interface IAwardService {
      
        Task<bool> IsAwardExistAsync(Guid id, CancellationToken cancellationToken = default);
        Task<AwardResponseDto> CreateAwardAsync(AwardRequestDto awardDto, CancellationToken cancellationToken = default);
        Task UpdateAwardAsync(Guid id, AwardUpdateDto awardUpdateDto, CancellationToken cancellationToken = default);
        Task UpdateAwardAvatarAsync(Guid id, IFormFile picture, CancellationToken cancellationToken = default);
        Task<FileDto> DownloadAwardAvatarAsync(Guid id, CancellationToken cancellationToken = default);
        Task DeleteAwardAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
