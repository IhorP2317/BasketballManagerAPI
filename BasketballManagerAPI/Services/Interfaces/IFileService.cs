using Security.Dto;

namespace Security.Services.Interfaces {
    public interface IFileService {
        Task RemoveFilesByNameIfExistsAsync(string fileName, CancellationToken cancellationToken = default);
        Task<string> StoreFileAsync(IFormFile file, string fileName, CancellationToken cancellationToken = default);
        Task<FileDto> GetFileAsync(string filePath, CancellationToken cancellationToken = default);
    }
}

