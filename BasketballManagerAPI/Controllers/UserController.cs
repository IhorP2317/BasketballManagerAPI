using BasketballManagerAPI.Dto.UserDto;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Helpers;
using BasketballManagerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BasketballManagerAPI.Controllers {
    [ApiController]
    [Route("api/users")]
    public class UserController:ControllerBase {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ValidateModel]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<UserResponseDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] UserFiltersDto userFiltersDto,
            CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllUsersAsync(userFiltersDto, cancellationToken);
            return users.Items.IsNullOrEmpty() ? NoContent() : Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,SuperAdmin, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdAsync(id, cancellationToken);
            return Ok(user);
        }
        
        [HttpPost("register")]
        [AllowAnonymous]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
       
        public async Task<IActionResult> RegisterUser([FromBody] UserSignUpDto userSignUpDto,
            CancellationToken cancellationToken) {
            var response = await _userService.RegisterUserAsync(userSignUpDto, cancellationToken);

            return Ok(response);
        }
        
        [HttpPost("register/admin")]
        [Authorize(Roles = "SuperAdmin")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponseDto))]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserSignUpDto userSignUpDto,
            CancellationToken cancellationToken) {
            var response = await _userService.RegisterUserAsync(userSignUpDto, cancellationToken, true);

            return Ok(response);
        }
        
        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "SuperAdmin, Admin, User")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> UpdateUser(
            [FromRoute] Guid id,
            [FromBody] UserUpdateDto userUpdateDto,
            CancellationToken cancellationToken) {

            await _userService.UpdateUserAsync(id, userUpdateDto, cancellationToken);
            return NoContent();
        }
        [Authorize(Roles = "User,SuperAdmin,Admin")]
        [HttpPatch("{id}/avatar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserAvatarAsync([FromRoute] Guid id,
            [FromForm] IFormFile picture,
            CancellationToken cancellationToken) {
            await _userService.UpdateUserAvatarAsync(id, picture, cancellationToken);
            return NoContent();

        }

        [HttpGet("{id}/avatar")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadUserAvatarAsync([FromRoute] Guid id,
            CancellationToken cancellationToken) {
            var fileDto = await _userService.DownloadUserAvatarAsync(id, cancellationToken);
            return File(fileDto.Content, fileDto.MimeType, fileDto.FileName);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id) {
           
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        
        [HttpGet("email/confirm")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] Guid userId, [FromQuery] string token) {
            await _userService.ConfirmEmail(userId, token);
            return Ok();
        }


    }
}
