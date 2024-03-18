using BasketballManagerAPI.Dto.CoachDto;
using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace BasketballManagerAPI.Controllers {
    [ApiController]
    [Route("api/teams/")]
    public class CoachController:ControllerBase {
        private readonly ICoachService _coachService;
        private readonly ITeamService _teamService;

        public CoachController(ICoachService coachService, ITeamService teamService)
        {
            _coachService = coachService;
            _teamService = teamService;
        }
       
        [HttpGet("coaches")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<CoachResponseDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllCoachesAsync([FromQuery] CoachFiltersDto coachFiltersDto, CancellationToken cancellationToken)
        {
            var coaches = await _coachService.GetAllCoachesAsync(coachFiltersDto,cancellationToken);
            return coaches.Items.IsNullOrEmpty() ? NoContent() : Ok(coaches);
        }
        
        [HttpGet("{teamId:guid}/coaches")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CoachResponseDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCoachesByTeamIdAsync([FromRoute] Guid teamId,
            CancellationToken cancellationToken)
        {
            var coaches = await _coachService.GetAllCoachesByTeamIdAsync(teamId, cancellationToken);
            return coaches.IsNullOrEmpty() ? NoContent() : Ok(coaches);
        }

        [HttpGet("coaches/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoachResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCoachAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var coach = await _coachService.GetCoachAsync(id, cancellationToken);
            return Ok(coach);
        }
        [HttpGet("coaches/{id:guid}/details")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoachDetailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCoachDetailAsync([FromRoute] Guid id, CancellationToken cancellationToken) {
            var coach = await _coachService.GetCoachDetailAsync(id, cancellationToken);
           
            return Ok(coach);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost("coaches")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CoachResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    
        public async Task<IActionResult> CreateCoachAsync([FromBody] CoachRequestDto coachDto,
            CancellationToken cancellationToken) {
            var createdPCoach = await _coachService.CreateCoachAsync(coachDto, cancellationToken);
            return Ok(createdPCoach);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut("coaches/{id}")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> UpdateCoachAsync([FromRoute] Guid id, CoachUpdateDto coachDto, CancellationToken cancellationToken) {
            await _coachService.UpdateCoachAsync(id, coachDto, cancellationToken);
            return NoContent();
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPatch("coaches/{id}/team")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> UpdateCoachTeamAsync([FromRoute] Guid id, CoachUpdateTeamDto coachDto, CancellationToken cancellationToken) {
            await _coachService.UpdateCoachTeamAsync(id, coachDto.NewTeamId, cancellationToken);
            return NoContent();
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPatch("coaches/{id}/avatar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCoachAvatarAsync([FromRoute] Guid id,
            [FromForm] IFormFile picture,
            CancellationToken cancellationToken) {
            await _coachService.UpdateCoachAvatarAsync(id, picture, cancellationToken);
            return NoContent();

        }

        [HttpGet("coaches/{id}/avatar")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadCoachAvatarAsync([FromRoute] Guid id,
            CancellationToken cancellationToken) {
            var fileDto = await _coachService.DownloadCoachAvatarAsync(id, cancellationToken);
            return File(fileDto.Content, fileDto.MimeType, fileDto.FileName);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete("coaches/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCoachAsync([FromRoute] Guid id, CancellationToken cancellationToken) {
            await _coachService.DeleteCoachAsync(id, cancellationToken);
            return NoContent();
        }

    }
}
