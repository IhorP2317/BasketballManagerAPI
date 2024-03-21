using BasketballManagerAPI.Dto.UserDto;
using BasketballManagerAPI.Helpers;
using Security.Dto;

namespace BasketballManagerAPI.Services.Interfaces {
    public interface IUserService {
        Task<PagedList<UserResponseDto>> GetAllUsersAsync(UserFiltersDto userFiltersDto,
            CancellationToken cancellationToken = default);
        Task<UserResponseDto> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsUserExistsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<UserResponseDto> RegisterUserAsync(UserSignUpDto userSignUpDto, CancellationToken cancellationToken = default,
            bool isAdmin = false);
        Task ConfirmEmail(Guid userId, string token, CancellationToken cancellationToken = default);
        Task UpdateUserAsync(Guid id, UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default);
        Task UpdateUserBalanceAsync(Guid id, decimal balance, CancellationToken cancellationToken = default);
        Task UpdateUserAvatarAsync(Guid id, IFormFile picture, CancellationToken cancellationToken = default);
        Task<FileDto> DownloadUserAvatarAsync(Guid id, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
