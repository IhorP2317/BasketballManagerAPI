using AutoMapper;
using BasketballManagerAPI.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Security.Data;
using Security.Dto;
using Security.Models;
using Security.Services.Interfaces;
using Security.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Security.Services.Implementations {
    public class UserService:IAuthService{
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;
        private readonly AuthSettings _authSettings;

        public UserService(IMapper mapper, UserManager<ApplicationUser> userManager, ApplicationDbContext context, ITokenGenerator tokenGenerator, IOptions<AuthSettings> authSettings, IEmailService emailService) {
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
            _tokenGenerator = tokenGenerator;
            _authSettings = authSettings.Value;
            _emailService = emailService;
        }

        public async Task<UserResponseDto> CreateUserAsync(UserSignUpDto userDto)
        {
            if(await _userManager.FindByEmailAsync(userDto.Email) != null)
                throw new AuthException("User is already exist!", StatusCodes.Status409Conflict);

            var user = _mapper.Map<ApplicationUser>(userDto);
            user.UserName = userDto.Email;
            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
                throw new AuthException("Could not create User!", StatusCodes.Status409Conflict);

            var createdUser = await _userManager.FindByEmailAsync(userDto.Email);
                var addedRole = await _userManager.AddToRoleAsync(createdUser, "User");
                if (!addedRole.Succeeded)
                    throw new AuthException("Could not give user role", StatusCodes.Status409Conflict);
                var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _emailService.SendEmail(user.Email, user.Id.ToString(), emailConfirmationToken);

            return _mapper.Map<UserResponseDto>(createdUser);
        }

        public async Task<TokenDto> LoginAsync(UserLoginDto userDto)
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);

            if (user == null)
                throw new NotFoundException($"User with email {userDto.Email} not found!");

            var validPassword = await _userManager.CheckPasswordAsync(user, userDto.Password);

            if (!validPassword)
                throw new AuthException($"User with {userDto.Email} unauthorized!", StatusCodes.Status401Unauthorized);


            user.RefreshToken = _tokenGenerator.GenerateRefreshToken(user);
            await _userManager.UpdateAsync(user);

            return new TokenDto {
                AccessToken = await _tokenGenerator.GenerateAccessToken(user),
                RefreshToken = user.RefreshToken
            };
        }

        public async Task<TokenDto> RefreshAccessTokenAsync(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.RefreshToken != tokenDto.RefreshToken)
                throw new AuthException("Invalid refresh token,", StatusCodes.Status401Unauthorized);
            user.RefreshToken = _tokenGenerator.GenerateRefreshToken(user);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new AuthException("Could not create refresh token!", StatusCodes.Status500InternalServerError);
            return new TokenDto
            {
                AccessToken = await _tokenGenerator.GenerateAccessToken(user),
                RefreshToken = user.RefreshToken
            };
        }
        public async Task<bool> VerifyEmailAsync(Guid userId, string confirmationToken) {
            confirmationToken = confirmationToken.Replace(' ', '+');
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.ConfirmEmailAsync(user, confirmationToken);
            return result.Succeeded;
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token) {
            var tokenValidationParameters = new TokenValidationParameters {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _authSettings.SymmetricSecurityKey,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

            return principal;
        }


        public Task<bool> IsUserExistAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserExistAsync(string email, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        
    }
}
