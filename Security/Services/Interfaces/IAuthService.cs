using Security.Dto;
using Security.Dto.PasswordDto;
using Security.Dto.UserDto;
using System.Threading;

namespace Security.Services.Interfaces
{
    public interface IAuthService {

        Task<UserResponseDto> RegisterAsync(UserSignUpDto userDto, string role = "User");
        Task<TokenDto> LoginAsync(UserLoginDto userDto);
        Task<TokenDto> RefreshAccessTokenAsync(TokenDto tokenDto);
        Task ConfirmEmailRequestAsync();
        Task ConfirmEmailAsync(Guid userId, string confirmationToken);
        Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task ChangePasswordAsync(ChangePasswordDto changePasswordDto);
    }
}
