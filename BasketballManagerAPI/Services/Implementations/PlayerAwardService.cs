using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;

namespace BasketballManagerAPI.Services.Implementations {
    public class PlayerAwardService : IAwardService {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPlayerService _playerService;

        public PlayerAwardService(ApplicationDbContext context, IMapper mapper, IPlayerService playerService) {
            _context = context;
            _playerService = playerService;
            _mapper = mapper;

        }
        public async Task<IEnumerable<AwardResponseDto>> GetAllAwardsByStaffIdAsync(Guid staffId, CancellationToken cancellationToken = default) {
            if (!await _playerService.IsPlayerExistAsync(staffId, cancellationToken))
                throw new NotFoundException($"Player with id {staffId} not found! ");

            var playerAwardsQuery = await _context.PlayerAwards
                .AsNoTracking()
                .Include(p => p.Award)
                .Include(p => p.Player)
                .Where(p => p.PlayerId == staffId)
                .Select(p => p.Award)
                .OrderBy(a => a.Name)
                .ThenBy(a => a.Date)
                .ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<AwardResponseDto>>(playerAwardsQuery);
        }

        public async Task<AwardResponseDto> GetAwardAsync(Guid staffId, Guid awardId, CancellationToken cancellationToken = default) {
            if (!await _playerService.IsPlayerExistAsync(staffId, cancellationToken))
                throw new NotFoundException($"Player with id {staffId} not found! ");
            if (!await IsAwardExistAsync(awardId, cancellationToken))
                throw new NotFoundException($"Award with id {awardId} not found! ");
            var award = await _context.PlayerAwards
                .AsNoTracking()
                .Include(p => p.Award)
                .Where(p => p.PlayerId == staffId)
                .Select(p => p.Award)
                .FirstOrDefaultAsync(p => p.Id == awardId, cancellationToken);
            if (award == null)
                throw new NotFoundException($"Player with id {staffId} does not have award with id {awardId}!");
            return _mapper.Map<AwardResponseDto>(award);
        }

        public async Task<bool> IsAwardExistAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _context.Awards.AnyAsync(a => a.Id == id, cancellationToken);
        }
        public async Task<bool> IsStaffHasAwardAsync(Guid staffId, Guid awardId, CancellationToken cancellationToken = default) {
            return await _context.PlayerAwards.AnyAsync(p => p.PlayerId == staffId && p.AwardId == awardId, cancellationToken);
        }

        public async Task<AwardResponseDto> CreateAwardAsync(Guid staffId, AwardRequestDto awardRequestDto,
            CancellationToken cancellationToken = default) {
            if (!await _playerService.IsPlayerExistAsync(staffId, cancellationToken))
                throw new NotFoundException($"Player with id {staffId} not found!");

            var award = await _context.Awards.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Name == awardRequestDto.Name && a.Date == DateOnly.Parse(awardRequestDto.Date), cancellationToken);

            if (award != null) {
                if (award.IsIndividualAward)
                    throw new DomainException("Award",
                        $"Award with name {award.Name} and date {award.Date} already exist, and it is individual!");
                if (awardRequestDto.IsIndividualAward.Value)
                    throw new DomainException("Award",
                        $"Award with name {award.Name} and date {award.Date} already exist. And it is common award but you try to create this award individually!");



            } else {
                award = _mapper.Map<Award>(awardRequestDto);
                _context.Awards.Add(award);
                await _context.SaveChangesAsync(cancellationToken);
            }

            if (await _context.PlayerAwards.AnyAsync(pa => pa.PlayerId == staffId && pa.AwardId == award.Id,
                    cancellationToken)) {
                throw new DomainException("PlayerAward", $"Player Id {staffId} and award id {award.Id}");
            }

            _context.PlayerAwards.Add(new PlayerAward {
                PlayerId = staffId,
                AwardId = award.Id,
            });

            await _context.SaveChangesAsync(cancellationToken);


            return _mapper.Map<AwardResponseDto>(award);
        }



        public async Task UpdateAwardAsync(Guid id, AwardUpdateDto awardUpdateDto, CancellationToken cancellationToken = default) {
            var foundedAward = await _context.Awards.FindAsync(id, cancellationToken);
            if (foundedAward == null)
                throw new NotFoundException($"Award with id {id} not found!");
            if (await _context.Awards
                   .AnyAsync(a => a.Name == awardUpdateDto.Name && a.Date == DateOnly.Parse(awardUpdateDto.Date) && a.Id != id, cancellationToken))
                throw new DomainException("Award",
                    $" Name {awardUpdateDto.Name} and award Date {awardUpdateDto.Date}");

            _mapper.Map(awardUpdateDto, foundedAward);
            await _context.SaveChangesAsync(cancellationToken);

        }

        public async Task DeleteAwardAsync(Guid staffId, Guid awardId, CancellationToken cancellationToken = default) {
            var foundedPlayerAward = await _context.PlayerAwards
                .Include(pa => pa.Award)
                .Include(pa => pa.Player)
                .FirstOrDefaultAsync(pa => pa.AwardId == awardId && pa.PlayerId == staffId, cancellationToken);

            if (foundedPlayerAward == null)
                throw new NotFoundException($"Association of player {staffId} with award {awardId} not found!");

            _context.PlayerAwards.Remove(foundedPlayerAward);
            await _context.SaveChangesAsync(cancellationToken);

            bool awardHasNoMoreAssociations = !await _context.PlayerAwards.AnyAsync(pa => pa.AwardId == awardId, cancellationToken) &&
                                              !await _context.CoachAwards.AnyAsync(c => c.AwardId == awardId, cancellationToken);
            if (awardHasNoMoreAssociations) {
                _context.Awards.Remove(foundedPlayerAward.Award);
            }


            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
