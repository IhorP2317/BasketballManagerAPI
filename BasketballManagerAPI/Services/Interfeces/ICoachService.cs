using BasketballManagerAPI.Dto.CoachDto;

namespace BasketballManagerAPI.Services.Interfeces {
    public interface ICoachService {
        Task<IEnumerable<CoachResponseDto>> GetAllCoachesAsync(CancellationToken  cancellationToken = default);
        Task<IEnumerable<CoachResponseDto>> GetAllCoachesByTeamIdAsync(Guid teamId, CancellationToken cancellationToken = default);
        Task<CoachResponseDto> GetCoachAsync(Guid id, CancellationToken cancellationToken = default);
        Task<CoachDetailDto> GetCoachDetailAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsCoachExistAsync(Guid id, CancellationToken cancellationToken = default);
        Task<CoachResponseDto> CreateCoachAsync(CoachRequestDto coachDto,  CancellationToken cancellationToken = default);
        Task UpdateCoachAsync(Guid id, CoachRequestDto coachDto, CancellationToken cancellationToken = default);
        Task DeleteCoachAsync(Guid id, CancellationToken cancellationToken = default);

    }
}
