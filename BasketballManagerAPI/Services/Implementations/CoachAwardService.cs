using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfaces;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;

namespace BasketballManagerAPI.Services.Implementations {
    public class CoachAwardService : IStaffAwardService {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICoachService _coachService;
        private readonly ICoachExperienceService _coachExperienceService;
        private readonly IAwardService _awardService;


        public CoachAwardService(ApplicationDbContext context, IMapper mapper, ICoachService coachService,
            ICoachExperienceService coachExperienceService,
            IAwardService awardService) {
            _context = context;
            _coachService = coachService;
            _mapper = mapper;
            _awardService = awardService;
            _coachExperienceService = coachExperienceService;

        }

        public async Task<IEnumerable<AwardResponseDto>> GetAllAwardsByStaffIdAsync(Guid staffId,
            CancellationToken cancellationToken = default) {
            if (!await _coachService.IsCoachExistAsync(staffId, cancellationToken))
                throw new NotFoundException($"Coach with id {staffId} not found! ");

            var coachAwardsQuery = await _context.CoachAwards
                .AsNoTracking()
                .Include(p => p.Award)
                .Include(p => p.CoachExperience)
                .Where(p => p.CoachExperience.CoachId == staffId)
                .Select(p => p.Award)
                .OrderBy(a => a.Name)
                .ThenBy(a => a.Date)
                .ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<AwardResponseDto>>(coachAwardsQuery);
        }

        public async Task<AwardResponseDto> GetAwardAsync(Guid staffExperienceId, Guid awardId,
            CancellationToken cancellationToken = default) {
            var award = await _context.CoachAwards
                .AsNoTracking()
                .Include(p => p.Award)
                .Where(p => p.CoachExperienceId == staffExperienceId)
                .Select(p => p.Award)
                .FirstOrDefaultAsync(p => p.Id == awardId, cancellationToken);
            if (award == null)
                throw new NotFoundException(
                    $"Coach experience with id {staffExperienceId} does not include award with id {awardId}!");
            return _mapper.Map<AwardResponseDto>(award);
        }


        public async Task<bool> IsStaffHasAwardAsync(Guid staffExperienceId, Guid awardId,
            CancellationToken cancellationToken = default) {
            return await _context.CoachAwards.AnyAsync(
                p => p.CoachExperienceId == staffExperienceId && p.AwardId == awardId, cancellationToken);
        }

        public async Task<AwardResponseDto> CreateAwardAsync(Guid staffExperienceId, AwardRequestDto awardRequestDto, CancellationToken cancellationToken = default) {
            var coachExperience = await _context.CoachExperiences
                .Include(pe => pe.Team)
                .FirstOrDefaultAsync(pe => pe.Id == staffExperienceId, cancellationToken);

            if (coachExperience == null) {
                throw new NotFoundException($"CoachExperience with id {staffExperienceId} not found!");
            }

            var existingAward = await FindOrAddAwardAsync(awardRequestDto, cancellationToken);
            if (existingAward.Id == Guid.Empty) {
                await _context.Awards.AddAsync(existingAward, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);


            }


            var alreadyAssigned = await _context.CoachAwards
                .AnyAsync(ca => ca.AwardId == existingAward.Id && ca.CoachExperienceId == staffExperienceId, cancellationToken);

            if (alreadyAssigned) {
                throw new DomainUniquenessException("CoachAward", $"Duplicate assignment of award {existingAward.Name} to coach experience {staffExperienceId}.");
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

                if (allRelatedTeams.Count > 1 || (allRelatedTeams.Count == 1 && allRelatedTeams.First() != coachExperience.TeamId)) {
                    throw new DomainLogicException("Award cannot be assigned to experiences from different teams.");
                }
            }

            if (existingAward.Date < coachExperience.StartDate || (existingAward.Date > (coachExperience.EndDate.HasValue ? coachExperience.EndDate.Value : DateOnly.FromDateTime(DateTime.Now)))) {
                throw new DomainLogicException("Award date must be within the duration of the coach's team experience.");
            }




            var coachAward = new CoachAward {
                CoachExperienceId = staffExperienceId,
                AwardId = existingAward.Id
            };

            await _context.CoachAwards.AddAsync(coachAward, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<AwardResponseDto>(existingAward);
        }

        private async Task<Award> FindOrAddAwardAsync(AwardRequestDto awardRequestDto, CancellationToken cancellationToken) {
            var existingAward = await _context.Awards.FirstOrDefaultAsync(a => a.Name == awardRequestDto.Name && a.Date == DateOnly.Parse(awardRequestDto.Date), cancellationToken);
            if (existingAward == null) {
                existingAward = _mapper.Map<Award>(awardRequestDto);
            } else {
                if (existingAward.IsIndividualAward) {
                    throw new DomainUniquenessException("Award", $"Award with name {existingAward.Name} and date {existingAward.Date} already exists with different individuality settings.");
                }

                if (awardRequestDto.IsIndividualAward.Value) {
                    throw new DomainUniquenessException("Award", $"Award with name {awardRequestDto.Name} and date {awardRequestDto.Date} already exists with different individuality settings.");
                }

            }
            return existingAward;
        }
        public async Task DeleteAwardAsync(Guid staffExperienceId, Guid awardId,
            CancellationToken cancellationToken = default) {
            var foundedCoachAward = await _context.CoachAwards
                .Include(pa => pa.Award)
                .Include(pa => pa.CoachExperience)
                .FirstOrDefaultAsync(pa => pa.AwardId == awardId && pa.CoachExperienceId == staffExperienceId,
                    cancellationToken);

            if (foundedCoachAward == null)
                throw new NotFoundException(
                    $"Association of coach experience {staffExperienceId} with award {awardId} not found!");

            _context.CoachAwards.Remove(foundedCoachAward);
            await _context.SaveChangesAsync(cancellationToken);


            bool awardHasNoMoreAssociations =
                !await _context.PlayerAwards.AnyAsync(pa => pa.AwardId == awardId, cancellationToken) &&
                !await _context.CoachAwards.AnyAsync(c => c.AwardId == awardId, cancellationToken);
            if (awardHasNoMoreAssociations)
                await _awardService.DeleteAwardAsync(foundedCoachAward.AwardId, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);


        }
    }
}
