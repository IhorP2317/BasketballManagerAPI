using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfaces;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Security.Dto;
using Security.Services.Interfaces;
using System;

namespace BasketballManagerAPI.Services.Implementations {
    public class PlayerAwardService : IStaffAwardService {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPlayerService _playerService;
        private readonly IPlayerExperienceService _playerExperienceService;
        private readonly IAwardService _awardService;


        public PlayerAwardService(ApplicationDbContext context, IMapper mapper, IPlayerService playerService,
            IPlayerExperienceService playerExperienceService,
            IAwardService awardService) {
            _context = context;
            _playerService = playerService;
            _mapper = mapper;
            _awardService = awardService;
            _playerExperienceService = playerExperienceService;

        }

        public async Task<IEnumerable<AwardResponseDto>> GetAllAwardsByStaffIdAsync(Guid staffId,
            CancellationToken cancellationToken = default) {
            if (!await _playerService.IsPlayerExistAsync(staffId, cancellationToken))
                throw new NotFoundException($"Player with id {staffId} not found! ");

            var playerAwardsQuery = await _context.PlayerAwards
                .AsNoTracking()
                .Include(p => p.Award)
                .Include(p => p.PlayerExperience)
                .Where(p => p.PlayerExperience.PlayerId == staffId)
                .Select(p => p.Award)
                .OrderBy(a => a.Name)
                .ThenBy(a => a.Date)
                .ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<AwardResponseDto>>(playerAwardsQuery);
        }

        public async Task<AwardResponseDto> GetAwardAsync(Guid staffExperienceId, Guid awardId,
            CancellationToken cancellationToken = default) {
            var award = await _context.PlayerAwards
                .AsNoTracking()
                .Include(p => p.Award)
                .Where(p => p.PlayerExperienceId == staffExperienceId)
                .Select(p => p.Award)
                .FirstOrDefaultAsync(p => p.Id == awardId, cancellationToken);
            if (award == null)
                throw new NotFoundException(
                    $"Player experience with id {staffExperienceId} does not include award with id {awardId}!");
            return _mapper.Map<AwardResponseDto>(award);
        }


        public async Task<bool> IsStaffHasAwardAsync(Guid staffExperienceId, Guid awardId,
            CancellationToken cancellationToken = default) {
            return await _context.PlayerAwards.AnyAsync(
                p => p.PlayerExperienceId == staffExperienceId && p.AwardId == awardId, cancellationToken);
        }

        public async Task<AwardResponseDto> CreateAwardAsync(Guid staffExperienceId, AwardRequestDto awardRequestDto, CancellationToken cancellationToken = default) {
            var playerExperience = await _context.PlayerExperiences
                .Include(pe => pe.Team)
                .FirstOrDefaultAsync(pe => pe.Id == staffExperienceId, cancellationToken);

            if (playerExperience == null) {
                throw new NotFoundException($"PlayerExperience with id {staffExperienceId} not found!");
            }

            var existingAward = await FindOrAddAwardAsync(awardRequestDto, cancellationToken);
           
           
            if (existingAward.Id == Guid.Empty) {
                await _context.Awards.AddAsync(existingAward, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);


            }

            var alreadyAssigned = await _context.PlayerAwards
                .AnyAsync(pa => pa.AwardId == existingAward.Id && pa.PlayerExperienceId == staffExperienceId, cancellationToken);

            if (alreadyAssigned) {
                throw new DomainUniquenessException("PlayerAward", $"Duplicate assignment of award {existingAward.Name} to player experience {staffExperienceId}.");
            }

            
            if (!existingAward.IsIndividualAward) {
                var relatedPlayerExperienceTeams = await _context.PlayerAwards
                    .Include(pa => pa.PlayerExperience)
                    .ThenInclude(pe => pe.Team)
                    .Where(pa => pa.AwardId == existingAward.Id)
                    .Select(pa => pa.PlayerExperience.TeamId)
                    .ToListAsync(cancellationToken);

                var relatedCoachExperienceTeams = await _context.CoachAwards
                    .Include(ca => ca.CoachExperience)
                    .ThenInclude(ce => ce.Team)
                    .Where(ca => ca.AwardId == existingAward.Id)
                    .Select(ca => ca.CoachExperience.TeamId)
                    .ToListAsync(cancellationToken);


                var allRelatedTeams = relatedPlayerExperienceTeams.Concat(relatedCoachExperienceTeams).Distinct().ToList();

                if (allRelatedTeams.Count > 1 || (allRelatedTeams.Count == 1 && allRelatedTeams.First() != playerExperience.TeamId)) {
                    throw new DomainLogicException("Award cannot be assigned to experiences from different teams.");
                }
            }

            if (existingAward.Date < playerExperience.StartDate || (existingAward.Date > (playerExperience.EndDate.HasValue ? playerExperience.EndDate.Value : DateOnly.FromDateTime(DateTime.Now)))) {
                throw new DomainLogicException("Award date must be within the duration of the player's team experience.");
            }




            var playerAward = new PlayerAward {
                PlayerExperienceId = staffExperienceId,
                AwardId = existingAward.Id
            };

            await _context.PlayerAwards.AddAsync(playerAward, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AwardResponseDto>(existingAward);
        }

        private async Task<Award> FindOrAddAwardAsync(AwardRequestDto awardRequestDto, CancellationToken cancellationToken) {
            var existingAward = await _context.Awards.FirstOrDefaultAsync(a => a.Name == awardRequestDto.Name && a.Date == DateOnly.Parse(awardRequestDto.Date), cancellationToken);
            if (existingAward == null) {
                existingAward = _mapper.Map<Award>(awardRequestDto);
            }
            else
            {
                if (existingAward.IsIndividualAward ) {
                    throw new DomainUniquenessException("Award", $"Award with name {existingAward.Name} and date {existingAward.Date} already exists with different individuality settings.");
                }

                if (awardRequestDto.IsIndividualAward.Value)
                {
                    throw new DomainUniquenessException("Award", $"Award with name {awardRequestDto.Name} and date {awardRequestDto.Date} already exists with different individuality settings.");
                }
            }
            
            return existingAward;
        }
        public async Task DeleteAwardAsync(Guid staffExperienceId, Guid awardId,
            CancellationToken cancellationToken = default) {
            var foundedPlayerAward = await _context.PlayerAwards
                .Include(pa => pa.Award)
                .Include(pa => pa.PlayerExperience)
                .FirstOrDefaultAsync(pa => pa.AwardId == awardId && pa.PlayerExperienceId == staffExperienceId,
                    cancellationToken);

            if (foundedPlayerAward == null)
                throw new NotFoundException(
                    $"Association of player experience {staffExperienceId} with award {awardId} not found!");

            _context.PlayerAwards.Remove(foundedPlayerAward);
            await _context.SaveChangesAsync(cancellationToken);


            bool awardHasNoMoreAssociations =
                !await _context.PlayerAwards.AnyAsync(pa => pa.AwardId == awardId, cancellationToken) &&
                !await _context.CoachAwards.AnyAsync(c => c.AwardId == awardId, cancellationToken);
            if (awardHasNoMoreAssociations)
                await _awardService.DeleteAwardAsync(foundedPlayerAward.AwardId, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);


        }
    }



}
