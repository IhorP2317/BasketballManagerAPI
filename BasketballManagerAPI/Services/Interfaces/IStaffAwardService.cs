using BasketballManagerAPI.Dto.AwardDto;

namespace BasketballManagerAPI.Services.Interfaces {
    public interface IStaffAwardService {
        Task<IEnumerable<AwardResponseDto>> GetAllAwardsByStaffIdAsync(Guid staffId,
            CancellationToken cancellationToken = default);
        Task<AwardResponseDto> GetAwardAsync(Guid staffExperienceId, Guid awardId, CancellationToken cancellationToken = default);
        Task<bool> IsStaffHasAwardAsync(Guid staffExperienceId, Guid awardId, CancellationToken cancellationToken = default);
        Task<AwardResponseDto> CreateAwardAsync(Guid staffExperienceId, AwardRequestDto awardRequestDto,
            CancellationToken cancellationToken = default);
        Task DeleteAwardAsync(Guid staffExperienceId, Guid awardId,
            CancellationToken cancellationToken = default);
    }
}
