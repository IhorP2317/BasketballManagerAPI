using BasketballManagerAPI.Dto.ExperienceDto;
using BasketballManagerAPI.Dto.PlayerExperienceDto;

namespace BasketballManagerAPI.Services.Interfeces {
    public interface IExperienceService
    {
        Task<IEnumerable<StaffExperienceResponseDto>> GetAllStaffExperienceAsync(Guid staffId,
            CancellationToken cancellationToken = default);

        Task<StaffExperienceResponseDto> GetStaffExperienceAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> IsStaffExperienceExistAsync(Guid id,
            CancellationToken cancellationToken = default);
        Task<StaffExperienceResponseDto> CreateExperienceAsync(StaffExperienceRequestDto staffExperienceRequestDto, CancellationToken cancellationToken = default);
        Task UpdateStaffExperienceAsync(Guid id,
            StaffExperienceRequestDto staffExperienceRequestDto, CancellationToken cancellationToken = default);
        Task DeleteStaffExperienceAsync(Guid id,CancellationToken  cancellationToken = default);
    }
}
