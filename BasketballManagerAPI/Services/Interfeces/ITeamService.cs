using BasketballManagerAPI.Dto.TeamDto;

namespace BasketballManagerAPI.Services.Interfeces {
    public interface ITeamService
    {
        Task<IEnumerable<TeamResponseDto>> GetAllTeamsAsync(CancellationToken cancellationToken = default);
        Task<TeamResponseDto> GetTeamAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TeamDetailDto> GetTeamDetailAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TeamResponseDto> GetTeamAsync(string name, CancellationToken cancellationToken = default);
      
        Task<bool> IsTeamExistsAsync(Guid id, CancellationToken cancellationToken = default);
       Task<bool> IsTeamExistsAsync(string name, CancellationToken cancellationToken = default);
        Task<TeamResponseDto> CreateTeamAsync(TeamRequestDto teamDto, CancellationToken cancellationToken = default);
        Task UpdateTeamAsync(Guid id, TeamRequestDto teamDto, CancellationToken cancellationToken = default);
        Task DeleteTeamAsync(Guid id, CancellationToken cancellationToken = default);


    }
}
