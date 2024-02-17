using BasketballManagerAPI.Dto.AwardDto;

namespace BasketballManagerAPI.Services.Interfeces {
    public interface IAwardService {
        Task<IEnumerable<AwardResponseDto>> GetAllAwardsByStaffIdAsync(Guid staffId,
            CancellationToken cancellationToken = default);
        Task<AwardResponseDto> GetAwardAsync(Guid staffId, Guid awardId, CancellationToken cancellationToken = default);
        Task<bool> IsAwardExistAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsStaffHasAwardAsync(Guid staffId, Guid awardId, CancellationToken cancellationToken = default);
        Task<AwardResponseDto> CreateAwardAsync(Guid staffId, AwardRequestDto awardRequestDto,
            CancellationToken cancellationToken = default);
        Task UpdateAwardAsync(Guid id, AwardUpdateDto awardUpdateDto, CancellationToken cancellationToken = default);
        Task DeleteAwardAsync(Guid staffId, Guid awardId,
            CancellationToken cancellationToken = default);


    }
}
