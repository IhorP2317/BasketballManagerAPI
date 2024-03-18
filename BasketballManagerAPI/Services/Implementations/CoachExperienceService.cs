using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.ExperienceDto;
using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfaces;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace BasketballManagerAPI.Services.Implementations {
    public class CoachExperienceService:ICoachExperienceService {
        private readonly ApplicationDbContext _context;
        private readonly ICoachService _coachService;
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;


        public CoachExperienceService(ApplicationDbContext context, ICoachService coachService, IMapper mapper, ITeamService teamService) {
            _context = context;
            _coachService = coachService;
            _teamService = teamService;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CoachExperienceResponseDto>> GetAllCoachExperiencesByCoachIdAsync(
            Guid coachId,
            CancellationToken cancellationToken = default) {
            if (!await _coachService.IsCoachExistAsync(coachId, cancellationToken))
                throw new NotFoundException($"Coach with id {coachId} does not exist!");
            var experiences = await _context.CoachExperiences.AsNoTracking()
                .Where(p => p.CoachId == coachId)
                .ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<CoachExperienceResponseDto>>(experiences);

        }

        public async Task<IEnumerable<CoachExperienceDetailDto>> GetAllCoachExperiencesByCoachIdDetailAsync(
            Guid coachId, CancellationToken cancellationToken = default) {
            if (!await _coachService.IsCoachExistAsync(coachId, cancellationToken))
                throw new NotFoundException($"Coach with id {coachId} does not exist!");
            var experiences = await _context.CoachExperiences.AsNoTracking()
                
                .Include(p => p.CoachAwards)
                .ThenInclude(p => p.Award)
                .Where(p => p.CoachId == coachId).ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<CoachExperienceDetailDto>>(experiences);
        }

        public async Task<CoachExperienceDetailDto> GetCoachExperienceDetailAsync(Guid id,
            CancellationToken cancellationToken = default) {
            var experience = await _context.CoachExperiences.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (experience == null)
                throw new NotFoundException($"Coach experience with id {id} does not exist!");
            return _mapper.Map<CoachExperienceDetailDto>(experience);
        }



        public async Task<bool> IsCoachExperienceExistAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _context.CoachExperiences.AnyAsync(p => p.Id == id, cancellationToken);
        }



        public async Task<CoachExperienceResponseDto> CreateExperienceAsync(Guid coachId, CoachExperienceRequestDto staffExperienceRequestDto,
            CancellationToken cancellationToken = default) {

            if (!await _teamService.IsTeamExistsAsync(staffExperienceRequestDto.TeamId, cancellationToken))
                throw new NotFoundException($"Team with id {staffExperienceRequestDto.TeamId} does note exist!");

            var coach = await _coachService.GetCoachAsync(coachId, cancellationToken);
            var currentExperience = _mapper.Map<CoachExperience>(staffExperienceRequestDto);
            currentExperience.CoachId = coachId;

            var existingExperiences = await GetAllCoachExperiencesByCoachIdAsync(coachId, cancellationToken);
            var sortedExperiences = existingExperiences.OrderBy(e => e.StartDate).ToList();
            var coach18ThBirthday = coach.DateOfBirth.AddYears(18);


            if (currentExperience.StartDate < coach18ThBirthday)
                throw new DomainLogicException("StartDate is older then start of coach professional career ");


            foreach (var experience in sortedExperiences) {
                var currentEndDateOrNow = currentExperience.EndDate.HasValue ? currentExperience.EndDate.Value : DateOnly.FromDateTime(DateTime.UtcNow);

                var experienceEndDateOrNow = experience.EndDate.HasValue ? experience.EndDate.Value : DateOnly.FromDateTime(DateTime.UtcNow);

                if (currentExperience.StartDate < experienceEndDateOrNow && currentEndDateOrNow > experience.StartDate) {
                    throw new DomainUniquenessException("New experience overlaps with an existing experience.", $"Start Date {currentExperience.StartDate} and End Date {(currentExperience.EndDate.HasValue ? currentExperience.EndDate.Value.ToString() : "N/A")}");
                }
            }
            if (!currentExperience.EndDate.HasValue) {
                if (!coach.TeamId.HasValue || currentExperience.TeamId != coach.TeamId)
                    throw new DomainLogicException("Outgoing experience should be related to the team that related with coach!");
                var lastExperience = sortedExperiences.LastOrDefault();
                if (lastExperience != null) {
                    if (currentExperience.StartDate < lastExperience.StartDate)
                        throw new DomainLogicException("Outgoing experience should be in present time!");
                }
                var ongoingExperience = sortedExperiences.LastOrDefault(e => !e.EndDate.HasValue);
                if (ongoingExperience != null) {
                    ongoingExperience.EndDate = currentExperience.StartDate.AddDays(-1);
                    var changedExperience = _mapper.Map<CoachExperience>(ongoingExperience);
                    _context.CoachExperiences.Update(changedExperience);
                }
            }
            var createdExperience = await _context.CoachExperiences.AddAsync(currentExperience, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<CoachExperienceResponseDto>(createdExperience.Entity);


        }


        public async Task UpdateCoachExperienceAsync(Guid id, CoachExperienceUpdateDto staffExperienceRequestDto,
            CancellationToken cancellationToken = default) {
            var foundedCoachExperience = await _context.CoachExperiences.FindAsync(id, cancellationToken);
            if (foundedCoachExperience == null)
                throw new NotFoundException($"Coach experience with id {id} does not exist!");
            if (foundedCoachExperience.EndDate != null)
                throw new DomainLogicException($"Coach experience with id {id} is not outgoing, so you can not change it!");
            if (!DateOnly.TryParse(staffExperienceRequestDto.EndDate, out var newExperienceEnd))
                throw new BadRequestException("Invalid format of end date!");

            if (newExperienceEnd < foundedCoachExperience.StartDate)
                throw new DomainLogicException(
                    $"Coach experience with id {id} can not have ned time {newExperienceEnd}, because it has start time {foundedCoachExperience.StartDate}!");

            foundedCoachExperience.EndDate = newExperienceEnd;
            _context.CoachExperiences.Update(foundedCoachExperience);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteCoachExperienceAsync(Guid id, CancellationToken cancellationToken = default) {
            var foundedCoachExperience = await _context.CoachExperiences.FindAsync(id, cancellationToken);
            if (foundedCoachExperience == null)
                throw new NotFoundException($"Coach experience with id {id} does not exist!");
            _context.CoachExperiences.Remove(foundedCoachExperience);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

