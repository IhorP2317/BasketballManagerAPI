using BasketballManagerAPI.Dto.CoachDto;
using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using BasketballManagerAPI.Filters;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CoachResponseDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllCoachesAsync(CancellationToken cancellationToken)
        {
            var coaches = await _coachService.GetAllCoachesAsync(cancellationToken);
            return coaches.IsNullOrEmpty() ? NoContent() : Ok(coaches);
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoachResponseDto))]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> UpdateCoachAsync([FromRoute] Guid id, CoachRequestDto coachDto, CancellationToken cancellationToken) {
            await _coachService.UpdateCoachAsync(id, coachDto, cancellationToken);
            return NoContent();
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
