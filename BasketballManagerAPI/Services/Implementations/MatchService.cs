using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.MatchDto;
using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Helpers;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace BasketballManagerAPI.Services.Implementations {
    public class MatchService: IMatchService {
        private readonly ApplicationDbContext _context;
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;

        public MatchService(ApplicationDbContext context, ITeamService teamService, IMapper mapper)
        {
            _context = context;
            _teamService = teamService;
            _mapper = mapper;
        }
        public async Task<PagedList<MatchResponseDto>> GetAllMatchesAsync(MatchFiltersDto matchFiltersDto, CancellationToken cancellationToken = default)
        {
            IQueryable<Match> matchesQuery = _context.Matches.AsNoTracking()
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.Statistics);

            matchesQuery = ApplyFilter(matchesQuery, matchFiltersDto);

            var sortedMatches = matchesQuery
                .OrderByDescending(m => m.Status == MatchStatus.Completed)
                .ThenByDescending(m => m.Status == MatchStatus.Completed ? m.StartTime : DateTime.MinValue)
                .ThenBy(m => m.Status != MatchStatus.Completed ? m.StartTime : DateTime.MaxValue);


            var matches = await PagedList<Match>.CreateAsync(
                matchesQuery,
                matchFiltersDto.Page,
                matchFiltersDto.PageSize,
                cancellationToken
            );
           
            return _mapper.Map<PagedList<MatchResponseDto>>(matches);
        }

        public async Task<MatchResponseDto> GetMatchAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var match = await _context
                .Matches
                .AsNoTracking()
                .Include(m => m.AwayTeam)
                .Include(m => m.HomeTeam)
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
            if (match == null)
                throw new NotFoundException($"Match with id {id} does not exist!");

            return _mapper.Map<MatchResponseDto>(match); ;

        }

        public async Task<MatchDetailDto> GetMatchDetailAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var match = await _context
                .Matches
                .Include(m => m.Statistics)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.Tickets)
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
            if (match == null)
                throw new NotFoundException($"Match with id {id} does not exist!");
            return _mapper.Map<MatchDetailDto>(match);
        }

        public async Task<bool> IsMatchExist(Guid id, CancellationToken cancellationToken = default)
        {
            return  await _context.Matches.AnyAsync(m => m.Id == id, cancellationToken);
        }

        public async  Task<MatchResponseDto> CreateMatchAsync(MatchRequestDto matchDto, CancellationToken cancellationToken = default)
        {
            if (!(await _teamService.IsTeamExistsAsync(matchDto.AwayTeamId, cancellationToken)
                  && await _teamService.IsTeamExistsAsync(matchDto.HomeTeamId, cancellationToken)))
                throw new NotFoundException("One of teams in match does not exist!");
            if (DateTime.TryParse(matchDto.StartTime, out DateTime startTime) &&
                !await IsTeamsAvailableForMatch(matchDto.HomeTeamId, matchDto.AwayTeamId, startTime, cancellationToken)) {
                throw new DomainException("Match", $"of Home team with id {matchDto.HomeTeamId} " +
                                                   $"or Away Team with id {matchDto.AwayTeamId} and StartTime {matchDto.StartTime} ");
            }

            var match = _mapper.Map<Match>(matchDto);
            var createdMatch = await _context.Matches.AddAsync(match, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<MatchResponseDto>(createdMatch.Entity);
        }

        public async Task UpdateMatchAsync(Guid id, MatchRequestDto matchDto, CancellationToken cancellationToken = default)
        {
            var foundedMatch = await _context.Matches.FindAsync(id, cancellationToken);
            if (foundedMatch == null)
                throw new NotFoundException($"Match with id {id} not found!");
            if (!(await _teamService.IsTeamExistsAsync(matchDto.AwayTeamId, cancellationToken)
                  && await _teamService.IsTeamExistsAsync(matchDto.HomeTeamId, cancellationToken)))
                throw new NotFoundException("One of teams in match does not exist!");
            if (DateTime.TryParse(matchDto.StartTime, out DateTime startTime) && 
                !await IsTeamsAvailableForMatch(matchDto.HomeTeamId, matchDto.AwayTeamId, startTime, cancellationToken))
            {
                throw new DomainException("Match", $"of Home team with id {matchDto.HomeTeamId} " +
                                                   $"or Away Team with id {matchDto.AwayTeamId} and StartTime {matchDto.StartTime} ");
            }

            _mapper.Map(matchDto, foundedMatch);

            await _context.SaveChangesAsync(cancellationToken);

        }

        public async Task DeleteMatchAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var foundedMatch = await _context.Matches.FindAsync(id, cancellationToken);
            if (foundedMatch == null)
                throw new NotFoundException($"Match with id {id} not found!");
            _context.Matches.Remove(foundedMatch);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task<bool> IsTeamsAvailableForMatch(Guid homeTeamId, Guid awayTeamId, DateTime startTime, CancellationToken cancellationToken) {
            return await _context.Matches
                .AnyAsync(m =>
                        m.StartTime == startTime &&
                        (m.HomeTeamId == homeTeamId || m.AwayTeamId == homeTeamId || m.HomeTeamId == awayTeamId || m.AwayTeamId == awayTeamId),
                    cancellationToken);
        }


        private IQueryable<Match> ApplyFilter(IQueryable<Match> matchesQuery, MatchFiltersDto matchFiltersDto) {
            if (!string.IsNullOrEmpty(matchFiltersDto.TeamName)) {
                matchesQuery = matchesQuery
                    .Where(m => m.HomeTeam.Name == matchFiltersDto.TeamName ||
                                m.AwayTeam.Name == matchFiltersDto.TeamName);
            }

            
            if (!string.IsNullOrEmpty(matchFiltersDto.Month) && int.TryParse(matchFiltersDto.Month, out int monthFilter)) {
                matchesQuery = matchesQuery.Where(m => m.StartTime.Month == monthFilter);
            } 

            
            if (!string.IsNullOrEmpty(matchFiltersDto.Year) && int.TryParse(matchFiltersDto.Year, out int yearFilter)) {
                matchesQuery = matchesQuery.Where(m => m.StartTime.Year == yearFilter);
            } 

            return matchesQuery;
        }

    }
}
