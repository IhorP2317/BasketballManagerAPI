using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Linq.Expressions;
using System.Security.AccessControl;
using BasketballManagerAPI.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace BasketballManagerAPI.Services.Implementations {
    public class PlayerService : IPlayerService {
        private readonly ApplicationDbContext _context;
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;

        public PlayerService(ApplicationDbContext applicationDbContext, IMapper mapper, ITeamService teamService ) {
            _context = applicationDbContext;
            _teamService = teamService;
            _mapper = mapper;
            
        }

        public async Task<PagedList<PlayerResponseDto>> GetAllPlayersAsync(PlayerFiltersDto playerFiltersDto, CancellationToken cancellationToken = default) {
            IQueryable<Player> playersQuery = _context.Players.AsNoTracking().Include(p => p.Team);

            playersQuery = ApplyFilter(playersQuery, playerFiltersDto);


            playersQuery = playerFiltersDto.SortOrder?.ToLower() == "desc"
                ? playersQuery.OrderByDescending(GetSortProperty(playerFiltersDto.SortColumn))
                : playersQuery.OrderBy(GetSortProperty(playerFiltersDto.SortColumn));


            var players = await PagedList<Player>.CreateAsync(
                playersQuery,
                playerFiltersDto.Page,
                playerFiltersDto.PageSize,
                cancellationToken
            );


            return _mapper.Map<PagedList<PlayerResponseDto>>(players);
        }

        public async Task<PlayerResponseDto> GetPlayerByIdAsync(Guid id, CancellationToken cancellationToken = default) {
            var player = await _context.Players.AsNoTracking().Include(p => p.Team).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (player == null)
                throw new NotFoundException($"Player with {id} does not exist!");
            return _mapper.Map<PlayerResponseDto>(player);
        }

        public async Task<PlayerDetailDto> GetPlayerByIdDetailAsync(Guid id, CancellationToken cancellationToken = default) {
            var player = await _context.Players.AsNoTracking()
                .Include(p => p.PlayerExperiences)
                .Include(p => p.Team)
                .Include(p => p.PlayerAwards)
                .ThenInclude(p => p.Award)
                .Include(p => p.Statistics)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (player == null)
                throw new NotFoundException($"Player with {id} does not exist!");
            return _mapper.Map<PlayerDetailDto>(player);
        }

        public async Task<bool> IsPlayerExistAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _context.Players.AnyAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<PlayerResponseDto> CreatePlayerAsync(PlayerRequestDto playerDto, CancellationToken cancellationToken = default) {
            if (playerDto.TeamId.HasValue) {

                if (!await _teamService.IsTeamExistsAsync(playerDto.TeamId.GetValueOrDefault(),
                        cancellationToken))
                    throw new NotFoundException("The team of created player does not exist!");
                if (await _context.Players.AnyAsync(
                        p => p.JerseyNumber == playerDto.JerseyNumber &&
                             p.TeamId.GetValueOrDefault() == playerDto.TeamId.GetValueOrDefault(), cancellationToken))
                    throw new DomainException("Player",
                        $"TeamId {playerDto.TeamId} and JerseyNumber {playerDto.JerseyNumber}");
            }
            var player = _mapper.Map<Player>(playerDto);
            var createdPlayer = await _context.Players.AddAsync(player, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<PlayerResponseDto>(createdPlayer.Entity);
        }

        public async Task UpdatePlayerAsync(Guid id, PlayerRequestDto playerDto, CancellationToken cancellationToken = default) {
            var foundedPlayer = await _context.Players.FindAsync(id, cancellationToken);
            if (foundedPlayer == null)
                throw new NotFoundException($"Player with ID {id} not found.");
            if (playerDto.TeamId.HasValue) {

                if (!await _teamService.IsTeamExistsAsync(playerDto.TeamId.GetValueOrDefault(),
                        cancellationToken))
                    throw new NotFoundException("The team of created player does not exist!");
                if (await _context.Players.AnyAsync(
                        p => p.JerseyNumber == playerDto.JerseyNumber &&
                             p.TeamId.GetValueOrDefault() == playerDto.TeamId.GetValueOrDefault() && p.Id != id, cancellationToken))
                    throw new DomainException("Player",
                        $"TeamId {playerDto.TeamId} and JerseyNumber {playerDto.JerseyNumber}");
            }

            _mapper.Map(playerDto, foundedPlayer);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeletePlayerAsync(Guid id, CancellationToken cancellationToken = default) {
            var foundedPlayer = await _context.Players.FindAsync(id, cancellationToken);
            if (foundedPlayer == null)
                throw new NotFoundException($"Player with ID {id} not found.");
            _context.Players.Remove(foundedPlayer);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<Player> ApplyFilter(IQueryable<Player> query, PlayerFiltersDto playerFiltersDto) {
            if (!string.IsNullOrEmpty(playerFiltersDto.LastName)) {
                query = query
                    .Where(p =>
                    EF.Functions.Like(p.LastName, $"{playerFiltersDto.LastName}%"));
            }

            if (!string.IsNullOrEmpty(playerFiltersDto.TeamName)) {
                query = query
                    .Include(p => p.Team)
                    .Where(p => p.Team != null && p.Team.Name == playerFiltersDto.TeamName);
            }

            if (!string.IsNullOrEmpty(playerFiltersDto.Position)) {
                if (Enum.TryParse(typeof(Position), playerFiltersDto.Position, out var positionValue)) {
                    query = query.Where(p => p.Position == (Position)positionValue);
                }
            }

            if (!string.IsNullOrEmpty(playerFiltersDto.Country)) {
                query = query.Where(p => p.Country == playerFiltersDto.Country);
            }

            return query;
        }
        private Expression<Func<Player, object>> GetSortProperty(string? sortColumn) {
            return sortColumn?.ToLower() switch {
                "lastName" => p => p.LastName,
                "firstName" => p => p.FirstName,
                "age" => p => EF.Functions.DateDiffYear(p.DateOfBirth, DateOnly.FromDateTime(DateTime.UtcNow)),
                "position" => p => p.Position,
                "country" => p => p.Country,
                "height" => p => p.Height,
                "weight" => p => p.Weight,
                "team" => p => p.Team.Name,
                _ => p => p.Id
            };
        }



    }
}
