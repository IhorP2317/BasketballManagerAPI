using BasketballManagerAPI.Dto.ExperienceDto;
using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Helpers;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BasketballManagerAPI.Controllers {
    [ApiController]
    [Route("api/players/")]
    public class PlayerExperienceController:ControllerBase {
        private readonly IPlayerExperienceService _playerExperienceService;

        public PlayerExperienceController(IPlayerExperienceService playerExperienceService)
        {
            _playerExperienceService = playerExperienceService;
        }

        [HttpGet("experiences")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<PlayerExperienceResponseDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllPlayersAsync([FromQuery] PlayerExperienceFiltersDto playerExperienceFilterDto, CancellationToken cancellationToken) {

            var players = await _playerExperienceService.GetAllPlayerExperiencesAsync(playerExperienceFilterDto, cancellationToken);
            return players.Items.IsNullOrEmpty() ? NoContent() : Ok(players);
        }

        [HttpGet("{id}/experiences")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PlayerExperienceResponseDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPlayerExperiencesByPlayerIdAsync([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var experiences =
                await _playerExperienceService.GetAllPlayerExperiencesByPlayerIdAsync(id, cancellationToken);
            return experiences.IsNullOrEmpty() ? NoContent() : Ok(experiences);
        }
        [HttpGet("{id}/experiences/details")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PlayerExperienceDetailDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPlayerExperiencesByPlayerIdDetailAsync([FromRoute] Guid id,
            CancellationToken cancellationToken) {
            var experiences =
                await _playerExperienceService.GetAllPlayerExperiencesByPlayerIdDetailAsync(id, cancellationToken);
            return experiences.IsNullOrEmpty() ? NoContent() : Ok(experiences);
        }
        [HttpGet("experiences/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlayerExperienceDetailDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlayerExperienceDetailAsync([FromRoute] Guid id,
            CancellationToken cancellationToken) {
            var experience = await _playerExperienceService.GetPlayerExperienceDetailAsync(id, cancellationToken);
            return  Ok(experience);
        }

        [HttpPost("{id}/experiences")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PlayerExperienceResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> CreatePlayerExperienceAsync([FromRoute] Guid id,
            [FromBody] PlayerExperienceRequestDto playerExperienceRequestDto, CancellationToken cancellationToken)
        {
            var createdExperience =
                await _playerExperienceService.CreateExperienceAsync(id, playerExperienceRequestDto, cancellationToken);
            return Ok(createdExperience);

        }

        [HttpPatch("experiences/{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> UpdatePlayerExperienceAsync([FromRoute] Guid id,
            [FromBody] PlayerExperienceUpdateDto playerExperienceUpdateDto, CancellationToken cancellationToken)
        {
            await _playerExperienceService.UpdatePlayerExperienceAsync(id, playerExperienceUpdateDto,
                cancellationToken);
            return NoContent();
        }

        [HttpDelete("experiences/{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePlayerExperienceAsync([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            await _playerExperienceService.DeletePlayerExperienceAsync(id, cancellationToken);
            return NoContent();
        }


    }
}
