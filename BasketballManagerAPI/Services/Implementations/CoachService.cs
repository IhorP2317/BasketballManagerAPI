using System.Linq.Expressions;
using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.CoachDto;
using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Security.Dto;
using Security.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Numerics;
using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Helpers;

namespace BasketballManagerAPI.Services.Implementations {
    public class CoachService : ICoachService {
        private readonly ApplicationDbContext _context;


        private readonly ITeamService _teamService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public CoachService(ApplicationDbContext context, ITeamService teamService, IMapper mapper, IFileService fileService) {
            _context = context;
            _teamService = teamService;
            _mapper = mapper;
            _fileService = fileService;
        }
        public async Task<PagedList<CoachResponseDto>> GetAllCoachesAsync(CoachFiltersDto coachFiltersDto, CancellationToken cancellationToken = default) {
            IQueryable<Coach> coachesQuery =  _context.Coaches.AsNoTracking().Include(c => c.Team);
            coachesQuery = ApplyFilter(coachesQuery, coachFiltersDto);


            coachesQuery = coachFiltersDto.SortOrder?.ToLower() == "desc"
                ? coachesQuery.OrderByDescending(GetSortProperty(coachFiltersDto.SortColumn))
                : coachesQuery.OrderBy(GetSortProperty(coachFiltersDto.SortColumn));


            var coaches = await PagedList<Coach>.CreateAsync(
                coachesQuery,
                coachFiltersDto.Page,
                coachFiltersDto.PageSize,
                cancellationToken
            );


            return _mapper.Map<PagedList<CoachResponseDto>>(coaches);

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
                .ThenInclude(p => p.CoachAwards)
                .FirstOrDefaultAsync(cancellationToken);
            if (coach == null)
                throw new NotFoundException($"Coach with id {id} does not exist!");
            return _mapper.Map<CoachDetailDto>(coach);
        }

        public async Task<bool> IsCoachExistAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _context.Coaches.AnyAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<CoachDetailDto> CreateCoachAsync(CoachRequestDto coachDto,
            CancellationToken cancellationToken = default) {
            var coach = _mapper.Map<Coach>(coachDto);
            if (coachDto.TeamId.HasValue) {
                if (!await _teamService.IsTeamExistsAsync(coachDto.TeamId.Value, cancellationToken))
                    throw new NotFoundException("The team of created coach does not exist!");
                if (coach.CoachStatus == CoachStatus.Head
                    && await _context.Coaches.AnyAsync(c => c.CoachStatus == coach.CoachStatus && c.TeamId == coach.TeamId,
                        cancellationToken))
                    throw new DomainUniquenessException($"Coach Status of Team with id {coachDto.TeamId}", coach.CoachStatus.ToString());
            }
            if (coachDto.CoachExperiences?.Any() == true) {
                var teams = await _teamService.GetAllTeamsAsync(cancellationToken);
                var teamsIds = teams.Select(t => t.Id).ToList();
                var allTeamExist = coachDto.CoachExperiences.All(p => teamsIds.Contains(p.TeamId));
                if (!allTeamExist)
                    throw new NotFoundException("Trying create coach with experience of not existing team!");
                coach.CoachExperiences = _mapper.Map<ICollection<CoachExperience>>(coachDto.CoachExperiences);
            }
            var createdCoach = await _context.Coaches.AddAsync(coach, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<CoachDetailDto>(createdCoach.Entity);
        }

        public async Task UpdateCoachAsync(Guid id, CoachUpdateDto coachDto,
            CancellationToken cancellationToken = default) {
            var foundedCoach = await _context.Coaches.FindAsync(id, cancellationToken);
            if (foundedCoach == null)
                throw new NotFoundException($"Coach with ID {id} not found.");
           
            if (coachDto.CoachStatus == "Head"
                && await _context.Coaches.AnyAsync(c => c.CoachStatus == CoachStatus.Head && c.TeamId == foundedCoach.TeamId && c.Id != id, cancellationToken))
                throw new DomainUniquenessException($"Coach Status of Team with id {foundedCoach.TeamId}", CoachStatus.Head);
            _mapper.Map(coachDto, foundedCoach);
            await _context.SaveChangesAsync(cancellationToken);

        }

        public async Task UpdateCoachTeamAsync(Guid coachId, Guid? newTeamId, CancellationToken cancellationToken = default)
        {
            var coach = await _context.Coaches
                .Include(c => c.CoachExperiences)
                .FirstOrDefaultAsync(c => c.Id == coachId, cancellationToken);

            if (coach == null) {
                throw new NotFoundException($"Coach with ID {coachId} not found.");
            }

            var currentExperience = coach.CoachExperiences
                .Where(pe => pe.EndDate == null)
                .SingleOrDefault();

            if (currentExperience != null) {

                currentExperience.EndDate = DateOnly.FromDateTime(DateTime.UtcNow);
            }

            if (newTeamId.HasValue) {

                if (!await _teamService.IsTeamExistsAsync(newTeamId.Value, cancellationToken))
                    throw new NotFoundException("The specified team does not exist!");

                var headCoachExists = coach.CoachStatus == CoachStatus.Head
                                      && await _context.Coaches.AnyAsync(
                                          c => c.CoachStatus == CoachStatus.Head && c.TeamId == newTeamId &&
                                               c.Id != coachId, cancellationToken);
                if (headCoachExists) 
                throw new DomainUniquenessException($"Coach Status of Team with id {newTeamId}", CoachStatus.Head);


                var newExperience = new CoachExperience {
                    CoachId = coachId,
                    TeamId = newTeamId.Value,
                    StartDate = DateOnly.FromDateTime(DateTime.UtcNow),

                };

                _context.CoachExperiences.Add(newExperience);
            }
            coach.TeamId = newTeamId;

            await _context.SaveChangesAsync(cancellationToken);
        }


        public async Task UpdateCoachAvatarAsync(Guid id, IFormFile picture,
            CancellationToken cancellationToken = default) {
            var foundedCoach = await _context.Coaches.FindAsync(id, cancellationToken);
            if (foundedCoach == null)
                throw new NotFoundException($"Coach with ID {id} not found.");
            if (picture.Length <= 0 || picture == null)
                throw new BadRequestException("Uploaded  image is  empty!");
            await _fileService.RemoveFilesByNameIfExistsAsync(fileName: id.ToString(), cancellationToken);
            var filePath = await _fileService.StoreFileAsync(picture, fileName: id.ToString(), cancellationToken);

            foundedCoach.PhotoPath = filePath;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<FileDto> DownloadCoachAvatarAsync(Guid id, CancellationToken cancellationToken = default) {
            var foundedCoach = await _context.Coaches.FindAsync(id, cancellationToken);
            if (foundedCoach == null)
                throw new NotFoundException($"Coach with ID {id} not found.");
            var filePath = foundedCoach.PhotoPath;

            if (filePath.IsNullOrEmpty())
                throw new NotFoundException($"Coach with ID {id} has not  this file path!");

            var fileDto = await _fileService.GetFileAsync(filePath, cancellationToken);
            return fileDto;
        }

        public async Task DeleteCoachAsync(Guid id, CancellationToken cancellationToken = default) {
            var foundedCoach = await _context.Coaches.FindAsync(id, cancellationToken);
            if (foundedCoach == null)
                throw new NotFoundException($"Coach with ID {id} not found.");
            _context.Coaches.Remove(foundedCoach);
            await _context.SaveChangesAsync(cancellationToken);
        }
        private IQueryable<Coach> ApplyFilter(IQueryable<Coach> query, CoachFiltersDto coachFiltersDto) {
            if (!string.IsNullOrEmpty(coachFiltersDto.LastName)) {
                query = query
                    .Where(c =>
                        EF.Functions.Like(c.LastName, $"{coachFiltersDto.LastName}%"));
            }

            if (!string.IsNullOrEmpty(coachFiltersDto.TeamName)) {
                query = query
                    .Include(p => p.Team)
                    .Where(c => c.Team != null && c.Team.Name == coachFiltersDto.TeamName);
            }

            if (!string.IsNullOrEmpty(coachFiltersDto.Status)) {
                if (Enum.TryParse(typeof(CoachStatus), coachFiltersDto.Status, true, out var statusValue)) {
                    query = query.Where(c => c.CoachStatus == (CoachStatus)statusValue);
                }
            }

            if (!string.IsNullOrEmpty(coachFiltersDto.Country)) {
                query = query.Where(c => c.Country == coachFiltersDto.Country);
            }

            return query;
        }
        private Expression<Func<Coach, object>> GetSortProperty(string? sortColumn) {
            return sortColumn?.ToLower() switch {
                "lastname" => c => c.LastName,
                "firstname" => c => c.FirstName,
                "age" => c => EF.Functions.DateDiffYear(c.DateOfBirth, DateOnly.FromDateTime(DateTime.UtcNow)),
                "status" => c => c.CoachStatus,
                "country" => c => c.Country,
                "team" => c => c.Team.Name,
                _ => c => c.Id
            };
        }
    }
}
