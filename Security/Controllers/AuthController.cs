using BasketballManagerAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Security.Dto;
using Security.Dto.PasswordDto;
using Security.Dto.UserDto;
using Security.Models;
using Security.Services.Implementations;
using Security.Services.Interfaces;

namespace Security.Controllers
{
    
    [ApiController]
    [Route(("api/auth"))]
    public class AuthController:ControllerBase {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("signUp")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignUpAsync([FromBody] UserSignUpDto userDto)
        {
            var createdUser = await _authService.RegisterAsync(userDto);
            return Ok(createdUser);
        }
        [HttpPost("signUp/admin")]
        [Authorize(Roles = "SuperAdmin")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignUpAdminAsync([FromBody] UserSignUpDto userDto) {
            var createdUser = await _authService.RegisterAsync(userDto, "Admin");
            return Ok(createdUser);
        }
        [HttpPost("login")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto userDto) {
            var confirmedUser = await _authService.LoginAsync(userDto);
            return Ok(confirmedUser);
        }

        [HttpPost("refresh")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshAccessTokeAsync([FromBody] TokenDto tokenDto)
        {
            var token = await _authService.RefreshAccessTokenAsync(tokenDto);
            return Ok(token);
        }

        [HttpGet("email/verification")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] Guid userId, [FromQuery] string token) {
            await _authService.ConfirmEmailAsync(userId, token);
            return Ok();
        }
        [HttpGet("email/verification/request")]
        [Authorize(Roles = "SuperAdmin, Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ConfirmEmailRequestAsync() {
            await _authService.ConfirmEmailRequestAsync();
            return Ok();
        }
        
        [HttpPost("password/forgot")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordDto forgotPasswordDto) {
            await _authService.ForgotPasswordAsync(forgotPasswordDto);
            return Ok();
        }


        [HttpPost("password/reset")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto) {
            await _authService.ResetPasswordAsync(resetPasswordDto);
            return Ok();
        }
        [HttpGet("reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetPassword([FromQuery] string userId, [FromQuery] string token)
        {
           var user = await  _userManager.FindByIdAsync(userId);
           if (user == null)
               return NotFound();
           var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var response = new ResetPasswordDto {
                UserId = userId,
                Token = resetToken
            };
            return Ok(response);
        }

        [HttpPost("password/change")]
        [Authorize(Roles = "SuperAdmin, Admin, User")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto) {
            await _authService.ChangePasswordAsync(changePasswordDto);
            return Ok();
        }


    }
}
