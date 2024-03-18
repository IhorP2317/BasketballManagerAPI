using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.TeamDto;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Helpers;
using Security.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Security.Dto;

namespace BasketballManagerAPI.Services.Implementations {
    public class TeamService : ITeamService {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public TeamService(ApplicationDbContext context, IFileService fileService, IMapper mapper) {
            _context = context;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TeamResponseDto>> GetAllTeamsAsync(CancellationToken cancellationToken = default) {
            var teams = await _context.Teams.AsNoTracking().ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TeamResponseDto>>(teams);
        }

        public async Task<PagedList<TeamResponseDto>> GetAllTeamsWithFiltersAsync(TeamFiltersDto teamFiltersDto, CancellationToken cancellationToken = default) {
            var teamsQuery = _context.Teams.AsNoTracking();
            if (!string.IsNullOrEmpty(teamFiltersDto.Name))
                teamsQuery = teamsQuery.Where(t => t.Name == teamFiltersDto.Name);
            teamsQuery = teamsQuery.OrderBy(t => t.Name);
            var teams = await PagedList<Team>.CreateAsync(
                teamsQuery,
                teamFiltersDto.Page,
                teamFiltersDto.PageSize,
                cancellationToken);
            return _mapper.Map<PagedList<TeamResponseDto>>(teams);

        }



        public async Task<TeamResponseDto> GetTeamAsync(Guid id, CancellationToken cancellationToken = default) {

            var team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (team == null)
                throw new NotFoundException($"Team with id {id} does not exist!");
            return _mapper.Map<TeamResponseDto>(team);

        }

        public async Task<TeamDetailDto> GetTeamDetailAsync(Guid id, CancellationToken cancellationToken = default) {
            var team = await _context.Teams
                .AsNoTracking()
                .Include(t => t.Players)
                .Include(t => t.Coaches)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (team == null)
                throw new NotFoundException($"Team with id {id} does not exist!");
            return _mapper.Map<TeamDetailDto>(team);

        }

        public async Task<bool> IsTeamExistsAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _context.Teams.AnyAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<TeamResponseDto> CreateTeamAsync(TeamRequestDto teamDto, CancellationToken cancellationToken = default) {

            var team = _mapper.Map<Team>(teamDto);
            if (await IsTeamExistsAsync(team.Id, cancellationToken))
                throw new DomainUniquenessException("TeamName", team.Name);
            var createdTeam = await _context.Teams.AddAsync(team, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<TeamResponseDto>(createdTeam.Entity);
        }

        public async Task UpdateTeamAsync(Guid id, TeamRequestDto teamDto, CancellationToken cancellationToken = default) {
            var foundedTeam = await _context.Teams.FindAsync(id, cancellationToken);
            if (foundedTeam == null) {
                throw new NotFoundException($"Team with ID {id} not found.");
            }

            if (await _context.Teams.AnyAsync(t => t.Name == teamDto.Name && t.Id != id, cancellationToken))
                throw new DomainUniquenessException("TeamName", teamDto.Name);
            _mapper.Map(teamDto, foundedTeam);
            await _context.SaveChangesAsync(cancellationToken);

        }
        public async Task UpdateTeamAvatarAsync(Guid id, IFormFile picture, CancellationToken cancellationToken = default) {
            var foundedTeam = await _context.Teams.FindAsync(id, cancellationToken);
            if (foundedTeam == null)
                throw new NotFoundException($"Team with ID {id} not found.");
            if (picture.Length <= 0 || picture == null)
                throw new BadRequestException("Uploaded  image is  empty!");
            await _fileService.RemoveFilesByNameIfExistsAsync(fileName: id.ToString(), cancellationToken);
            var filePath = await _fileService.StoreFileAsync(picture, fileName: id.ToString(), cancellationToken);

            foundedTeam.PhotoPath = filePath;

            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<FileDto> DownloadTeamAvatarAsync(Guid id, CancellationToken cancellationToken = default) {
            var foundedTeam = await _context.Teams.FindAsync(id, cancellationToken);
            if (foundedTeam == null)
                throw new NotFoundException($"Team with ID {id} not found.");
            var filePath = foundedTeam.PhotoPath;

            if (filePath.IsNullOrEmpty())
                throw new NotFoundException($"Team with ID {id} has not  this file path!");

            var fileDto = await _fileService.GetFileAsync(filePath, cancellationToken);
            return fileDto;
        }

        public async Task DeleteTeamAsync(Guid id, CancellationToken cancellationToken = default) {
            var foundedTeam = await _context.Teams.FindAsync(id, cancellationToken);
            if (foundedTeam == null) {
                throw new NotFoundException($"Team with ID {id} not found.");
            }

            _context.Teams.Remove(foundedTeam);
            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
