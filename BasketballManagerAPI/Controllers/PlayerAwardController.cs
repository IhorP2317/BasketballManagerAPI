using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfaces;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BasketballManagerAPI.Controllers {
    [ApiController]
    [Route("api/")]
    public class PlayerAwardController:ControllerBase {
        private readonly IStaffAwardService _playerAwardService;


        public PlayerAwardController( IStaffAwardServiceFactory staffAwardServiceFactory)
        {
            _playerAwardService = staffAwardServiceFactory.CreatePlayerAwardService();
        }
        [HttpGet("players/{id:guid}/experiences/awards")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AwardResponseDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPlayerAwardsAsync([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var awards = await _playerAwardService.GetAllAwardsByStaffIdAsync(id, cancellationToken);
            return awards.IsNullOrEmpty() ? NoContent() : Ok(awards);
        }
        [HttpGet("players/experiences/{playerExperienceId:guid}/awards/{awardId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AwardResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAwardAsync([FromRoute] Guid playerExperienceId, [FromRoute] Guid awardId,CancellationToken cancellationToken) {
            var award = await _playerAwardService.GetAwardAsync(playerExperienceId, awardId, cancellationToken );
            return Ok(award);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost("players/experiences/{id:guid}/awards")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PlayerResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateAwardAsync([FromRoute] Guid id,
            [FromBody] AwardRequestDto awardRequestDto, CancellationToken cancellationToken)
        {
            var createdAward = await _playerAwardService.CreateAwardAsync(id, awardRequestDto, cancellationToken);
            return Ok(createdAward);

        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete("players/experiences/{id:guid}/awards/{awardId:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAwardAsync([FromRoute] Guid id, [FromRoute] Guid awardId,
            CancellationToken cancellationToken)
        {
            await _playerAwardService.DeleteAwardAsync(id, awardId, cancellationToken);
            return NoContent();
        }

    }
}
