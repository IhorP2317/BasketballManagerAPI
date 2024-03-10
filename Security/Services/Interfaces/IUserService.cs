using Security.Dto;
using System.Threading;

namespace Security.Services.Interfaces {
    public interface IUserService {

        Task<UserResponseDto> CreateUserAsync(UserSignUpDto userDto);
        Task<TokenDto> LoginAsync(UserLoginDto userDto);
        Task<TokenDto> RefreshAccessTokenAsync(TokenDto tokenDto);

    }
}
