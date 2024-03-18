using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Dto.CoachDto;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BasketballManagerAPI.Controllers {
    [ApiController]
    [Route("api/")]
    public class CoachAwardController : ControllerBase {
        
       
        private readonly IStaffAwardService _coachAwardService;

        public CoachAwardController( IStaffAwardServiceFactory awardServiceFactory) {
           
            _coachAwardService = awardServiceFactory.CreateCoachAwardService();
        }
        [HttpGet("coaches/{id:guid}/experiences/awards")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AwardResponseDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCoachAwardsAsync([FromRoute] Guid id,
            CancellationToken cancellationToken) {
            var awards = await _coachAwardService.GetAllAwardsByStaffIdAsync(id, cancellationToken);
            return awards.IsNullOrEmpty() ? NoContent() : Ok(awards);
        }
        [HttpGet("coaches/experiences/{coachExperienceId:guid}/awards/{awardId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AwardResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAwardAsync([FromRoute] Guid coachExperienceId, [FromRoute] Guid awardId, CancellationToken cancellationToken) {
            var award = await _coachAwardService.GetAwardAsync(coachExperienceId, awardId, cancellationToken);
            return Ok(award);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost("coaches/experiences/{id:guid}/awards")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CoachResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateAwardAsync([FromRoute] Guid id,
            [FromBody] AwardRequestDto awardRequestDto, CancellationToken cancellationToken) {
            var createdAward = await _coachAwardService.CreateAwardAsync(id, awardRequestDto, cancellationToken);
            return Ok(createdAward);

        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete("coaches/experiences/{id:guid}/awards/{awardId:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAwardAsync([FromRoute] Guid id, [FromRoute] Guid awardId,
            CancellationToken cancellationToken) {
            await _coachAwardService.DeleteAwardAsync(id, awardId, cancellationToken);
            return NoContent();
        }

    }
}
