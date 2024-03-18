using Security.Dto.User;

namespace Security.Services.Interfaces {
    public interface IUserService
    {
        Task DeleteUserAsync(Guid userId);
        Task UpdateUserAsync(Guid userId, UserUpdateDto userUpdateDto);
    }
}
