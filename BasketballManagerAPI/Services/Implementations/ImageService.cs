using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Helpers;
using Microsoft.AspNetCore.StaticFiles;
using Security.Dto;
using Security.Services.Interfaces;

namespace BasketballManagerAPI.Services.Implementations {
    public class ImageService : IFileService {

        private readonly IConfiguration _configuration;


        public ImageService(IConfiguration configuration) {
            _configuration = configuration;
        }
        public async Task RemoveFilesByNameIfExistsAsync(string fileName, CancellationToken cancellationToken = default) {
            var folderName = _configuration["FileStorage:FolderPath"];
            var pathToRemoveFiles = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            var filesToDelete = Directory.GetFiles(pathToRemoveFiles, fileName + ".*");
            var deleteTasks = filesToDelete
                .Select(
                    fileToDelete => Task.Run(
                        () => File.Delete(
                            fileToDelete
                        ),
                        cancellationToken
                    )
                );

            await Task.WhenAll(deleteTasks);
        }

        public async Task<string> StoreFileAsync(IFormFile file, string fileName,
            CancellationToken cancellationToken = default) {
            if (!FileHelper.IsImageFile(file.FileName))
                throw new BadRequestException("The format of file is not an image!");
            var folderName = _configuration["FileStorage:FolderPath"];
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fileExtension = Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(pathToSave, fileName + fileExtension);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);

            return fullPath;
        }

        public async Task<FileDto> GetFileAsync(string filePath, CancellationToken cancellationToken = default) {
            if (!File.Exists(filePath))
                throw new NotFoundException("File doesn't exists");

            var contentTypeProvider = new FileExtensionContentTypeProvider();
            contentTypeProvider.TryGetContentType(filePath, out string mimeType);

            return new FileDto {
                Content = await File.ReadAllBytesAsync(filePath, cancellationToken),
                FileName = "photo",
                MimeType = mimeType
            };
        }
    }
}

