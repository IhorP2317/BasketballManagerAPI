using BasketballManagerAPI.Dto.ExperienceDto;

namespace BasketballManagerAPI.Services.Interfaces {
    public interface ICoachExperienceService {
        Task<IEnumerable<CoachExperienceResponseDto>> GetAllCoachExperiencesByCoachIdAsync(Guid coachId,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<CoachExperienceDetailDto>> GetAllCoachExperiencesByCoachIdDetailAsync(Guid coachId,
            CancellationToken cancellationToken = default);
        Task<CoachExperienceDetailDto> GetCoachExperienceDetailAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> IsCoachExperienceExistAsync(Guid id,
            CancellationToken cancellationToken = default);
        Task<CoachExperienceResponseDto> CreateExperienceAsync(Guid coachId, CoachExperienceRequestDto coachExperienceRequestDto, CancellationToken cancellationToken = default);
        Task UpdateCoachExperienceAsync(Guid id,
            CoachExperienceUpdateDto coachExperienceUpdateDto, CancellationToken cancellationToken = default);
        Task DeleteCoachExperienceAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
