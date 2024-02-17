using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.CoachDto;
using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace BasketballManagerAPI.Services.Implementations {
    public class CoachService : ICoachService {
        private readonly ApplicationDbContext _context;


        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;

        public CoachService(ApplicationDbContext context, ITeamService teamService, IMapper mapper) {
            _context = context;
            _teamService = teamService;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CoachResponseDto>> GetAllCoachesAsync(CancellationToken cancellationToken = default) {
            var coaches = await _context.Coaches.AsNoTracking().ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<CoachResponseDto>>(coaches);
        }

        public async Task<IEnumerable<CoachResponseDto>> GetAllCoachesByTeamIdAsync(Guid teamId, CancellationToken cancellationToken = default) {
            if (!await _teamService.IsTeamExistsAsync(teamId, cancellationToken))
                throw new NotFoundException($"Team with id {teamId} does not exist!");
            var coaches = await _context.Coaches.AsNoTracking().Where(c => c.TeamId == teamId).ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<CoachResponseDto>>(coaches);
        }

        public async Task<CoachResponseDto> GetCoachAsync(Guid id, CancellationToken cancellationToken = default) {
            var coach = await _context.Coaches.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
            if (coach == null)
                throw new NotFoundException($"Coach with id {id} does not exist!");
            return _mapper.Map<CoachResponseDto>(coach);
        }

        public async Task<CoachDetailDto> GetCoachDetailAsync(Guid id, CancellationToken cancellationToken = default) {
            var coach = await _context.Coaches
                .AsNoTracking()
                .Include(c => c.Team)
                .Include(c => c.CoachExperiences)
                .Include(c => c.CoachExperiences)
                .FirstOrDefaultAsync(cancellationToken);
            if (coach == null)
                throw new NotFoundException($"Coach with id {id} does not exist!");
            return _mapper.Map<CoachDetailDto>(coach);
        }

        public async Task<bool> IsCoachExistAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _context.Coaches.AnyAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<CoachResponseDto> CreateCoachAsync(CoachRequestDto coachDto, CancellationToken cancellationToken = default) {
            var coach = _mapper.Map<Coach>(coachDto);
            if (coachDto.TeamId.HasValue && !await _teamService.IsTeamExistsAsync(coachDto.TeamId.Value, cancellationToken))
                throw new NotFoundException("The team of created coach does not exist!");
            if (coach.CoachStatus == CoachStatus.Head
                && await _context.Coaches.AnyAsync(c => c.CoachStatus == coach.CoachStatus && c.TeamId == coach.TeamId,
                    cancellationToken))
                throw new DomainException($"Coach Status of Team with id {coachDto.TeamId}", coach.CoachStatus.ToString());
            var createdCoach = await _context.Coaches.AddAsync(coach, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<CoachResponseDto>(createdCoach.Entity);
        }

        public async Task UpdateCoachAsync(Guid id, CoachRequestDto coachDto, CancellationToken cancellationToken = default) {
            var foundedCoach = await _context.Coaches.FindAsync(id, cancellationToken);
            if (foundedCoach == null)
                throw new NotFoundException($"Coach with ID {id} not found.");
            if (coachDto.TeamId.HasValue && !await _teamService.IsTeamExistsAsync(coachDto.TeamId.Value, cancellationToken))
                throw new NotFoundException("The team of created coach does not exist!");
            if (coachDto.CoachStatus == "Head"
                && await _context.Coaches.AnyAsync(c => c.CoachStatus == CoachStatus.Head && c.TeamId == coachDto.TeamId && c.Id != id, cancellationToken))
                throw new DomainException($"Coach Status of Team with id {coachDto.TeamId}", coachDto.CoachStatus.ToString());
            _mapper.Map(coachDto, foundedCoach);
            await _context.SaveChangesAsync(cancellationToken);

        }

        public async Task DeleteCoachAsync(Guid id, CancellationToken cancellationToken = default) {
            var foundedCoach = await _context.Coaches.FindAsync(id, cancellationToken);
            if (foundedCoach == null)
                throw new NotFoundException($"Coach with ID {id} not found.");
            _context.Coaches.Remove(foundedCoach);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
