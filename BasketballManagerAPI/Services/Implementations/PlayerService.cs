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
using Security.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Security.Dto;

namespace BasketballManagerAPI.Services.Implementations {
    public class PlayerService : IPlayerService {
        private readonly ApplicationDbContext _context;
        private readonly ITeamService _teamService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public PlayerService(ApplicationDbContext applicationDbContext, IMapper mapper, ITeamService teamService, IFileService fileService ) {
            _context = applicationDbContext;
            _teamService = teamService;
            _fileService = fileService;
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
                .ThenInclude(p => p.PlayerAwards)
                .Include(p => p.PlayerExperiences)
                .ThenInclude(p => p.Statistics)
                .Include(p => p.Team)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (player == null)
                throw new NotFoundException($"Player with {id} does not exist!");
            return _mapper.Map<PlayerDetailDto>(player);
        }

        public async Task<bool> IsPlayerExistAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _context.Players.AnyAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<PlayerDetailDto> CreatePlayerAsync(PlayerRequestDto playerDto, CancellationToken cancellationToken = default) {

            if (playerDto.TeamId.HasValue) {
                if (!await _teamService.IsTeamExistsAsync(playerDto.TeamId.Value, cancellationToken))
                    throw new NotFoundException("The team of created player does not exist!");

                
                if (await _context.Players.AnyAsync(
                        p => p.JerseyNumber == playerDto.JerseyNumber &&
                             p.TeamId == playerDto.TeamId, cancellationToken))
                    throw new DomainUniquenessException("Player",
                        $"TeamId {playerDto.TeamId} and JerseyNumber {playerDto.JerseyNumber}");
            }

            var player = _mapper.Map<Player>(playerDto);
            if (playerDto.PlayerExperiences?.Any() == true)
            {
                var teams = await _teamService.GetAllTeamsAsync(cancellationToken);
                var teamsIds = teams.Select(t => t.Id).ToList();
                var allTeamExist = playerDto.PlayerExperiences.All(p => teamsIds.Contains(p.TeamId));
                if (!allTeamExist)
                    throw new NotFoundException("Trying create player with experience of not existing team!");

                player.PlayerExperiences = _mapper.Map<ICollection<PlayerExperience>>(playerDto.PlayerExperiences);
            }
            var createdPlayer = await _context.AddAsync(player, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<PlayerDetailDto>(createdPlayer.Entity);
        }
        public async Task UpdatePlayerAsync(Guid id, PlayerUpdateDto playerDto,
            CancellationToken cancellationToken = default) {
            var foundedPlayer = await _context.Players
                .AsNoTracking()
                .Where(p => p.Id == id)
                .SingleOrDefaultAsync(cancellationToken);

            if (foundedPlayer == null)
                throw new NotFoundException($"Player with ID {id} not found.");

            if (foundedPlayer.TeamId != null)
            {
                var existingTeamId = foundedPlayer.TeamId;

                var isJerseyNumberTaken = await _context.Players.AnyAsync(
                    p => p.JerseyNumber == playerDto.JerseyNumber &&
                         p.TeamId == existingTeamId &&
                         p.Id != id,
                    cancellationToken);

                if (isJerseyNumberTaken)
                    throw new DomainUniquenessException("Player",
                        $"TeamId {existingTeamId} and JerseyNumber {playerDto.JerseyNumber} are already taken by another player.");

            }

            _mapper.Map(playerDto, foundedPlayer); 

            _context.Players.Update(foundedPlayer);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdatePlayerTeamAsync(Guid playerId, Guid? newTeamId, CancellationToken cancellationToken = default) {
            var player = await _context.Players
                .Include(p => p.PlayerExperiences)
                .FirstOrDefaultAsync(p => p.Id == playerId, cancellationToken);

            if (player == null) {
                throw new NotFoundException($"Player with ID {playerId} not found.");
            }

            var currentExperience = player.PlayerExperiences
                .Where(pe => pe.EndDate == null)
                .SingleOrDefault();

            if (currentExperience != null) {
              
                currentExperience.EndDate = DateOnly.FromDateTime(DateTime.UtcNow);
            }

            if (newTeamId.HasValue) {
                
                if (!await _teamService.IsTeamExistsAsync(newTeamId.Value, cancellationToken))
                    throw new NotFoundException("The specified team does not exist!");

                var jerseyNumberExists = await _context.Players.AnyAsync(
                    p => p.TeamId == newTeamId && p.JerseyNumber == player.JerseyNumber && p.Id != playerId,
                    cancellationToken);

                if (jerseyNumberExists)
                    throw new DomainUniquenessException("Player", $"Jersey Number {player.JerseyNumber} is already taken in the specified team.");

                
                var newExperience = new PlayerExperience {
                    PlayerId = playerId,
                    TeamId = newTeamId.Value,
                    StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    
                };

                _context.PlayerExperiences.Add(newExperience);
            }
            player.TeamId = newTeamId;

            await _context.SaveChangesAsync(cancellationToken);
        }


        public async Task UpdatePlayerAvatarAsync(Guid id, IFormFile picture, CancellationToken cancellationToken = default)
        {
            var foundedPlayer = await _context.Players.FindAsync(id, cancellationToken);
            if (foundedPlayer == null)
                throw new NotFoundException($"Player with ID {id} not found.");
            if (picture.Length <= 0 || picture == null)
                throw new BadRequestException("Uploaded  image is  empty!");
            await _fileService.RemoveFilesByNameIfExistsAsync(fileName: id.ToString(), cancellationToken);
            var filePath = await _fileService.StoreFileAsync(picture, fileName: id.ToString(), cancellationToken);

            foundedPlayer.PhotoPath = filePath;

            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<FileDto> DownloadPlayerAvatarAsync(Guid id, CancellationToken cancellationToken = default) {
            var foundedPlayer = await _context.Players.FindAsync(id, cancellationToken);
            if (foundedPlayer == null)
                throw new NotFoundException($"Player with ID {id} not found.");
            var filePath = foundedPlayer.PhotoPath;

            if (filePath.IsNullOrEmpty())
                throw new NotFoundException($"Player with ID {id} has not  this file path!");

            var fileDto = await _fileService.GetFileAsync(filePath, cancellationToken);
            return fileDto;
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
                "lastname" => p => p.LastName,
                "firstname" => p => p.FirstName,
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
