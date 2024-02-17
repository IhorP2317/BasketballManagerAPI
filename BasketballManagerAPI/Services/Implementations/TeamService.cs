using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.TeamDto;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using BasketballManagerAPI.Exceptions;

namespace BasketballManagerAPI.Services.Implementations {
    public class TeamService:ITeamService {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TeamService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TeamResponseDto>> GetAllTeamsAsync(CancellationToken cancellationToken = default)
        {
            var teams = await _context.Teams.AsNoTracking().ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TeamResponseDto>>(teams);
        }

      

        public  async Task<TeamResponseDto> GetTeamAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            return _mapper.Map<TeamResponseDto>(team);

        }

        public async Task<TeamDetailDto> GetTeamDetailAsync(Guid id, CancellationToken cancellationToken = default) {
            var team = await _context.Teams
                .AsNoTracking()
                .Include(t => t.Players)
                .Include(t => t.Coaches)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            return _mapper.Map<TeamDetailDto>(team);

        }


        public async Task<TeamResponseDto> GetTeamAsync(string name, CancellationToken cancellationToken = default)
        {
            var team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(e => e.Name == name, cancellationToken );
            return _mapper.Map<TeamResponseDto>(team);
        }
        

        public async Task<bool> IsTeamExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Teams.AnyAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<bool> IsTeamExistsAsync(string name, CancellationToken cancellationToken = default) {
            return await _context.Teams.AnyAsync(e => e.Name.Equals(name), cancellationToken);
        }

        public async Task<TeamResponseDto> CreateTeamAsync(TeamRequestDto teamDto, CancellationToken cancellationToken = default)
        {
           
            var team = _mapper.Map<Team>(teamDto);
            if(await IsTeamExistsAsync(team.Name, cancellationToken))
               throw new DomainException("TeamName", team.Name);
            var createdTeam = await _context.Teams.AddAsync(team, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<TeamResponseDto>(createdTeam.Entity);
        }

        public async Task UpdateTeamAsync(Guid id, TeamRequestDto teamDto, CancellationToken cancellationToken = default)
        {
            var foundedTeam = await _context.Teams.FindAsync(id, cancellationToken);
            if (foundedTeam == null) {
               throw new NotFoundException($"Team with ID {id} not found.");
            }

            if(await _context.Teams.AnyAsync(t => t.Name == teamDto.Name && t.Id != id, cancellationToken))
                throw new DomainException("TeamName", teamDto.Name);
                _mapper.Map(teamDto, foundedTeam);
             await _context.SaveChangesAsync(cancellationToken);

        }

        public async Task DeleteTeamAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var foundedTeam = await _context.Teams.FindAsync(id, cancellationToken);
            if (foundedTeam == null) {
                throw new NotFoundException($"Team with ID {id} not found.");
            }

            _context.Teams.Remove(foundedTeam);
             await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
