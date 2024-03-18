﻿using AutoMapper;
using BasketballManagerAPI.Exceptions;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Security.Data;
using Security.Dto;
using Security.Dto.UserDto;
using Security.Models;
using Security.Services.Interfaces;
using Security.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Security.Dto.PasswordDto;
using Security.Helpers;

namespace Security.Services.Implementations
{
    public class AuthService:IAuthService{
        private readonly AuthSettings _authSettings;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly MailTemplatesConstants _mailTemplatesConstants;
        public AuthService(IMapper mapper, UserManager<ApplicationUser> userManager,
            ITokenGenerator tokenGenerator, IOptions<AuthSettings> authSettings,
            IEmailService emailService, IHttpContextAccessor httpContextAccessor, ICurrentUserService currentUserService,
            IWebHostEnvironment webHostEnvironment, IOptions<MailTemplatesConstants> mailTemplatesConstants) {
            _mapper = mapper;
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;
            _webHostEnvironment = webHostEnvironment;
            _mailTemplatesConstants = mailTemplatesConstants.Value;
            _authSettings = authSettings.Value;
        }


        public async Task<UserResponseDto> RegisterAsync(UserSignUpDto userSignUpDto, string role = "User") {
            var user = _mapper.Map<ApplicationUser>(userSignUpDto);
            user.UserName = user.Email;

            var result = await _userManager.CreateAsync(user, userSignUpDto.Password);

            if (result.Succeeded) {
                await _userManager.AddToRoleAsync(user, role);
            } else {
                throw new AuthException(result.Errors.First().Description, StatusCodes.Status409Conflict);
            }

            var sendingResult = await SendEmailVerificationMailAsync(user);
            if (!sendingResult)
                throw new AuthException("Failed to send an email", StatusCodes.Status500InternalServerError);

            return _mapper.Map<UserResponseDto>(userSignUpDto);
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

        public async Task ConfirmEmailRequestAsync()
        {
            var user = await _userManager.FindByEmailAsync(_currentUserService.Email);
            if (!user.EmailConfirmed) {
                var sendingResult = await SendEmailVerificationMailAsync(user);
                if (!sendingResult)
                    throw new AuthException("Failed to send an email", StatusCodes.Status500InternalServerError);
            }
        }

       
        public async Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                throw new NotFoundException($"User with email {forgotPasswordDto.Email} does not exist!");
            var result = await SendResetPasswordMailAsync(user);
            if (!result)
                throw new AuthException("Failed to send an email for reset.", StatusCodes.Status500InternalServerError);

        }

        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            resetPasswordDto.Token = resetPasswordDto.Token.Replace(' ','+');
            var user = await _userManager.FindByIdAsync(resetPasswordDto.UserId);
            if (user == null)
                throw new NotFoundException("User with such id not found");

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);

            if (!result.Succeeded)
                throw new AuthException(result.Errors.First().Description, StatusCodes.Status500InternalServerError);
        }

        public async Task ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            if (user == null)
                throw new NotFoundException($"User with  id {_currentUserService.UserId} does not found!");
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword,
                changePasswordDto.NewPassword);
            if (!result.Succeeded)
                throw new AuthException(result.Errors.First().Description, StatusCodes.Status500InternalServerError);

        }

        public async Task ConfirmEmailAsync(Guid userId, string confirmationToken){
            confirmationToken = confirmationToken.Replace(' ', '+');
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new NotFoundException($"User with id {userId} does not exist!");
            var result = await _userManager.ConfirmEmailAsync(user, confirmationToken);
            if(!result.Succeeded)
                throw new AuthException(result.Errors.First().Description, StatusCodes.Status500InternalServerError);
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

        private async Task<bool> SendEmailVerificationMailAsync(ApplicationUser user) {
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var subject = _mailTemplatesConstants.VerifyEmailMailSubject;
            var mailTemplatePath = _mailTemplatesConstants.VerifyEmailMailTemplatePath;
            var endpoint = "email/verification";

            return await SendEmailWithToken(user, emailConfirmationToken, subject, mailTemplatePath, endpoint);
        }
        private async Task<bool> SendResetPasswordMailAsync(ApplicationUser user) {
            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var subject = _mailTemplatesConstants.ForgotPasswordMailSubject;
            var mailTemplatePath = _mailTemplatesConstants.ForgotPasswordMailTemplatePath;
            var endpoint = "reset-password";

            return await SendEmailWithToken(user, passwordResetToken, subject, mailTemplatePath, endpoint);
        }

        private async Task<bool> SendEmailWithToken(ApplicationUser user, string token, string subject,
            string mailTemplatePath, string endpoint) {
            var basePath = _webHostEnvironment.WebRootPath ?? _webHostEnvironment.ContentRootPath;
            var emailTemplatePath = Path.Combine(basePath, mailTemplatePath);
            var emailHtmlContent = await File.ReadAllTextAsync(emailTemplatePath);

            var scheme = _httpContextAccessor.HttpContext.Request.Scheme;
            var host = _httpContextAccessor.HttpContext.Request.Host.Value;
            var verificationUrl = $"{scheme}://{host}/api/auth/{endpoint}"
                                  + $"?userId={user.Id}&token={token}";

            emailHtmlContent = emailHtmlContent.Replace("{url}", verificationUrl);

            return await _emailService.SendEmail(
                user.Email,
                subject,
                emailHtmlContent
            );
        }


        
    }
}