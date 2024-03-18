using AutoMapper;
using BasketballManagerAPI.Data;
using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Security.Dto;
using Security.Services.Interfaces;

namespace BasketballManagerAPI.Services.Implementations {
    public class AwardService:IAwardService {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        public AwardService(ApplicationDbContext context, IMapper mapper, IFileService fileService) {
            _context = context;
            _mapper = mapper;
            _fileService = fileService;
        }
        public async Task<bool> IsAwardExistAsync(Guid id, CancellationToken cancellationToken = default) {
            return await _context.Awards.AnyAsync(a => a.Id == id, cancellationToken);
        }

        public  async Task<AwardResponseDto> CreateAwardAsync(AwardRequestDto awardDto, CancellationToken cancellationToken = default)
        {
            if(await _context.Awards
                   .AnyAsync(a => a.Name == awardDto.Name && a.Date == DateOnly.Parse(awardDto.Date), cancellationToken))
                throw new DomainUniquenessException("Award",
                    $" Name {awardDto.Name} and award Date {awardDto.Date}");
            var  award = _mapper.Map<Award>(awardDto);
            var createdAward = await _context.Awards.AddAsync(award, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<AwardResponseDto>(createdAward.Entity);
        }

        public async Task UpdateAwardAsync(Guid id, AwardUpdateDto awardUpdateDto, CancellationToken cancellationToken = default) {
            var foundedAward = await _context.Awards.FindAsync(id, cancellationToken);
            if (foundedAward == null)
                throw new NotFoundException($"Award with id {id} not found!");
            if (await _context.Awards
                   .AnyAsync(a => a.Name == awardUpdateDto.Name && a.Date == DateOnly.Parse(awardUpdateDto.Date) && a.Id != id, cancellationToken))
                throw new DomainUniquenessException("Award",
                    $" Name {awardUpdateDto.Name} and award Date {awardUpdateDto.Date}");

            _mapper.Map(awardUpdateDto, foundedAward);
            await _context.SaveChangesAsync(cancellationToken);

        }
        public async Task UpdateAwardAvatarAsync(Guid id, IFormFile picture, CancellationToken cancellationToken = default) {
            var foundedAward = await _context.Awards.FindAsync(id, cancellationToken);
            if (foundedAward == null)
                throw new NotFoundException($"Award with ID {id} not found.");
            if (picture.Length <= 0 || picture == null)
                throw new BadRequestException("Uploaded  image is  empty!");
            await _fileService.RemoveFilesByNameIfExistsAsync(fileName: id.ToString(), cancellationToken);
            var filePath = await _fileService.StoreFileAsync(picture, fileName: id.ToString(), cancellationToken);

            foundedAward.PhotoPath = filePath;

            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<FileDto> DownloadAwardAvatarAsync(Guid id, CancellationToken cancellationToken = default) {
            var foundedAward = await _context.Awards.FindAsync(id, cancellationToken);
            if (foundedAward == null)
                throw new NotFoundException($"Award with ID {id} not found.");
            var filePath = foundedAward.PhotoPath;

            if (filePath.IsNullOrEmpty())
                throw new NotFoundException($"Award with ID {id} has not  this file path!");

            var fileDto = await _fileService.GetFileAsync(filePath, cancellationToken);
            return fileDto;
        }

        public async Task DeleteAwardAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var foundedAward = await _context.Awards.FindAsync(id, cancellationToken);
            if (foundedAward == null)
                throw new NotFoundException($"Award with ID {id} not found.");
            _context.Awards.Remove(foundedAward);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
