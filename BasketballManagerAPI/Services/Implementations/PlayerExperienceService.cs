using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.ExperienceDto;
using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using BasketballManagerAPI.Dto.PlayerDto;
using System.Linq.Expressions;
using BasketballManagerAPI.Helpers;

namespace BasketballManagerAPI.Services.Implementations {
    public class PlayerExperienceService : IPlayerExperienceService {
        private readonly ApplicationDbContext _context;
        private readonly IPlayerService _playerService;
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;


        public PlayerExperienceService(ApplicationDbContext context, IPlayerService playerService, IMapper mapper, ITeamService teamService) {
            _context = context;
            _playerService = playerService;
            _teamService = teamService;
            _mapper = mapper;
        }

        public async Task<PagedList<PlayerExperienceResponseDto>> GetAllPlayerExperiencesAsync(PlayerExperienceFiltersDto playerExperienceFilterDto,
            CancellationToken cancellationToken = default)
        {
            IQueryable<PlayerExperience> experienceQuery =  _context.PlayerExperiences.AsNoTracking()
                .Include(p => p.Statistics)
                .ThenInclude(s => s.Match);
            experienceQuery = ApplyFilter(experienceQuery, playerExperienceFilterDto);
            experienceQuery = experienceQuery.OrderBy(p => p.Team.Name)
                .ThenBy(p => p.StartDate);
            var experiences = await PagedList<PlayerExperience>.CreateAsync(
                experienceQuery,
                playerExperienceFilterDto.Page,
                playerExperienceFilterDto.PageSize,
                cancellationToken
            );


            return _mapper.Map<PagedList<PlayerExperienceResponseDto>>(experiences);
        }


        public async Task<IEnumerable<PlayerExperienceResponseDto>> GetAllPlayerExperiencesByPlayerIdAsync(
            Guid playerId,
            CancellationToken cancellationToken = default) {
            if (!await _playerService.IsPlayerExistAsync(playerId, cancellationToken))
                throw new NotFoundException($"Player with id {playerId} does not exist!");
            var experiences = await _context.PlayerExperiences.AsNoTracking()
                .Where(p => p.PlayerId == playerId)
                .ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PlayerExperienceResponseDto>>(experiences);

        }

        public async Task<IEnumerable<PlayerExperienceDetailDto>> GetAllPlayerExperiencesByPlayerIdDetailAsync(
            Guid playerId, CancellationToken cancellationToken = default) {
            if (!await _playerService.IsPlayerExistAsync(playerId, cancellationToken))
                throw new NotFoundException($"Player with id {playerId} does not exist!");
            var experiences = await _context.PlayerExperiences.AsNoTracking()
                .Include(p => p.Statistics)
                .Include(p => p.PlayerAwards)
                .ThenInclude(p => p.Award)
                .Where(p => p.PlayerId == playerId).ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PlayerExperienceDetailDto>>(experiences);
        }

        public async Task<PlayerExperienceDetailDto> GetPlayerExperienceDetailAsync(Guid id,
            CancellationToken cancellationToken = default) {
            var experience = await _context.PlayerExperiences.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (experience == null)
                throw new NotFoundException($"Player experience with id {id} does not exist!");
            return _mapper.Map<PlayerExperienceDetailDto>(experience);
        }



        public async Task<bool> IsPlayerExperienceExistAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _context.PlayerExperiences.AnyAsync(p => p.Id == id, cancellationToken);
        }

        

        public async Task<PlayerExperienceResponseDto> CreateExperienceAsync(Guid playerId, PlayerExperienceRequestDto staffExperienceRequestDto,
            CancellationToken cancellationToken = default) {

            if (!await _teamService.IsTeamExistsAsync(staffExperienceRequestDto.TeamId, cancellationToken))
                throw new NotFoundException($"Team with id {staffExperienceRequestDto.TeamId} does note exist!");

            var player = await _playerService.GetPlayerByIdAsync(playerId, cancellationToken);
            var currentExperience = _mapper.Map<PlayerExperience>(staffExperienceRequestDto);
            currentExperience.PlayerId = playerId;

            var existingExperiences = await GetAllPlayerExperiencesByPlayerIdAsync(playerId, cancellationToken);
            var sortedExperiences = existingExperiences.OrderBy(e => e.StartDate).ToList();
            var player18ThBirthday = player.DateOfBirth.AddYears(18);


            if (currentExperience.StartDate < player18ThBirthday)
                throw new DomainLogicException("StartDate is older then start of player professional career ");


            foreach (var experience in sortedExperiences) {
                var currentEndDateOrNow = currentExperience.EndDate.HasValue ? currentExperience.EndDate.Value : DateOnly.FromDateTime(DateTime.UtcNow);

                
                var experienceEndDateOrNow = experience.EndDate.HasValue ? experience.EndDate.Value : DateOnly.FromDateTime(DateTime.UtcNow);

                if (currentExperience.StartDate < experienceEndDateOrNow && currentEndDateOrNow > experience.StartDate) {
                    throw new DomainUniquenessException("New experience overlaps with an existing experience.", $"Start Date {currentExperience.StartDate} and End Date {(currentExperience.EndDate.HasValue ? currentExperience.EndDate.Value.ToString() : "N/A")}");
                }

            }
            if (!currentExperience.EndDate.HasValue) {
                if(!player.TeamId.HasValue || currentExperience.TeamId != player.TeamId)
                    throw new DomainLogicException("Outgoing experience should be related to the team that related to player!");
                var lastExperience = sortedExperiences.LastOrDefault();
                if (lastExperience != null) {
                    if (currentExperience.StartDate < lastExperience.StartDate)
                        throw new DomainLogicException("Outgoing experience should be in present time!");
                }
                var ongoingExperience = sortedExperiences.LastOrDefault(e => !e.EndDate.HasValue);
                if (ongoingExperience != null) {
                    ongoingExperience.EndDate = currentExperience.StartDate.AddDays(-1);
                    var changedExperience = _mapper.Map<PlayerExperience>(ongoingExperience);
                    _context.PlayerExperiences.Update(changedExperience);
                }
            }
            var createdExperience = await _context.PlayerExperiences.AddAsync(currentExperience, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<PlayerExperienceResponseDto>(createdExperience.Entity);


        }


        public async Task UpdatePlayerExperienceAsync(Guid id, PlayerExperienceUpdateDto staffExperienceRequestDto,
            CancellationToken cancellationToken = default)
        {
            var foundedPlayerExperience = await _context.PlayerExperiences.FindAsync(id, cancellationToken);
            if (foundedPlayerExperience == null)
                throw new NotFoundException($"Player experience with id {id} does not exist!");
            if(foundedPlayerExperience.EndDate != null)
                throw new DomainLogicException($"Player experience with id {id} is not outgoing, so you can not change it!");
            if (!DateOnly.TryParse(staffExperienceRequestDto.EndDate, out var newExperienceEnd))
                throw new BadRequestException("Invalid format of end date!");

            if (newExperienceEnd < foundedPlayerExperience.StartDate)
                    throw new DomainLogicException(
                        $"Player experience with id {id} can not have ned time {newExperienceEnd}, because it has start time {foundedPlayerExperience.StartDate}!");

            foundedPlayerExperience.EndDate = newExperienceEnd;
            _context.PlayerExperiences.Update(foundedPlayerExperience);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeletePlayerExperienceAsync(Guid id, CancellationToken cancellationToken = default) {
            var foundedPlayerExperience = await _context.PlayerExperiences.FindAsync(id, cancellationToken);
            if (foundedPlayerExperience == null)
                throw new NotFoundException($"Player experience with id {id} does not exist!");
            _context.PlayerExperiences.Remove(foundedPlayerExperience);

            await _context.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<PlayerExperience> ApplyFilter(IQueryable<PlayerExperience> query,
            PlayerExperienceFiltersDto playerExperienceFilterDto)
        {
            if (!string.IsNullOrEmpty(playerExperienceFilterDto.MatchStartTime) && DateTime.TryParse(playerExperienceFilterDto.MatchStartTime, out var matchStartTime)) {
                
                var matchStartDate = DateOnly.FromDateTime(matchStartTime);

                query = query.Where(p =>
                    p.StartDate <= matchStartDate &&
                    (!p.EndDate.HasValue || p.EndDate >= matchStartDate));
            }

          
            if (playerExperienceFilterDto.TeamId.HasValue) {
                query = query.Where(p => p.TeamId == playerExperienceFilterDto.TeamId.Value);
            }
            return query;
        }
       
    }
}
