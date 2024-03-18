using BasketballManagerAPI.Dto.CoachDto;
using BasketballManagerAPI.Helpers;
using Security.Dto;

namespace BasketballManagerAPI.Services.Interfeces {
    public interface ICoachService {
        Task<PagedList<CoachResponseDto>> GetAllCoachesAsync(CoachFiltersDto coachFiltersDto, CancellationToken  cancellationToken = default);
        Task<IEnumerable<CoachResponseDto>> GetAllCoachesByTeamIdAsync(Guid teamId, CancellationToken cancellationToken = default);
        Task<CoachResponseDto> GetCoachAsync(Guid id, CancellationToken cancellationToken = default);
        Task<CoachDetailDto> GetCoachDetailAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsCoachExistAsync(Guid id, CancellationToken cancellationToken = default);
        Task<CoachDetailDto> CreateCoachAsync(CoachRequestDto coachDto, CancellationToken cancellationToken = default);
        Task UpdateCoachAsync(Guid id, CoachUpdateDto coachDto, CancellationToken cancellationToken = default);
        Task UpdateCoachTeamAsync(Guid coachId, Guid? newTeamId, CancellationToken cancellationToken = default);
        Task UpdateCoachAvatarAsync(Guid id, IFormFile picture, CancellationToken cancellationToken = default);
        Task<FileDto> DownloadCoachAvatarAsync(Guid id, CancellationToken cancellationToken = default);
        Task DeleteCoachAsync(Guid id, CancellationToken cancellationToken = default);

    }
}
