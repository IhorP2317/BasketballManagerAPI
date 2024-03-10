using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BasketballManagerAPI.Controllers {
    [ApiController]
    [Route("api/players/")]
    public class PlayerAwardController:ControllerBase {
        private readonly IAwardService _awardService;


        public PlayerAwardController(IAwardService awardService)
        {
            _awardService = awardService;
        }
        [HttpGet("{id:guid}/awards")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AwardResponseDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPlayerAwardsAsync([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var awards = await _awardService.GetAllAwardsByStaffIdAsync(id, cancellationToken);
            return awards.IsNullOrEmpty() ? NoContent() : Ok(awards);
        }
        [HttpGet("{playerId:guid}/awards/{awardId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AwardResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAwardAsync([FromRoute] Guid playerId, [FromRoute] Guid awardId,CancellationToken cancellationToken) {
            var award = await _awardService.GetAwardAsync(playerId, awardId, cancellationToken );
            return Ok(award);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost("{id:guid}/awards")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PlayerResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateAwardAsync([FromRoute] Guid id,
            [FromBody] AwardRequestDto awardRequestDto, CancellationToken cancellationToken)
        {
            var createdAward = await _awardService.CreateAwardAsync(id, awardRequestDto, cancellationToken);
            return Ok(createdAward);

        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut("awards/{id:guid}")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<IActionResult> UpdateAwardAsync([FromRoute] Guid id,
            [FromBody] AwardUpdateDto awardUpdateDto, CancellationToken cancellationToken) {
            await _awardService.UpdateAwardAsync(id, awardUpdateDto, cancellationToken);
            return NoContent();

        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete("{playerId:guid}/awards/{awardId:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAwardAsync([FromRoute] Guid playerId, [FromRoute] Guid awardId,
            CancellationToken cancellationToken)
        {
            await _awardService.DeleteAwardAsync(playerId, awardId, cancellationToken);
            return NoContent();
        }

    }
}
