using BasketballManagerAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using Security.Dto;
using Security.Services.Interfaces;

namespace Security.Controllers {
    [ApiController]
    [Route(("api/auth"))]
    public class AuthController:ControllerBase {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signUp")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignUpAsync([FromBody] UserSignUpDto userDto)
        {
            var createdUser = await _userService.CreateUserAsync(userDto);
            return Ok(createdUser);
        }
        [HttpPost("login")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto userDto) {
            var confirmedUser = await _userService.LoginAsync(userDto);
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
            var token = await _userService.RefreshAccessTokenAsync(tokenDto);
            return Ok(token);

        }




    }
}
