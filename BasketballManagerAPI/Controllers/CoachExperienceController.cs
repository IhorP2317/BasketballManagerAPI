using BasketballManagerAPI.Dto.ExperienceDto;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Services.Interfaces;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BasketballManagerAPI.Controllers {
    [ApiController]
    [Route("api/teams/coaches")]
    public class CoachExperienceController:ControllerBase {
            private readonly ICoachExperienceService _coachExperienceService;

            public CoachExperienceController(ICoachExperienceService coachExperienceService) {
                _coachExperienceService = coachExperienceService;
            }

            [HttpGet("{id}/experiences")]
            [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CoachExperienceResponseDto>))]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> GetAllCoachExperiencesByCoachIdAsync([FromRoute] Guid id,
                CancellationToken cancellationToken) {
                var experiences =
                    await _coachExperienceService.GetAllCoachExperiencesByCoachIdAsync(id, cancellationToken);
                return experiences.IsNullOrEmpty() ? NoContent() : Ok(experiences);
            }
            [HttpGet("{id}/experiences/details")]
            [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CoachExperienceDetailDto>))]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> GetAllCoachExperiencesByCoachIdDetailAsync([FromRoute] Guid id,
                CancellationToken cancellationToken) {
                var experiences =
                    await _coachExperienceService.GetAllCoachExperiencesByCoachIdDetailAsync(id, cancellationToken);
                return experiences.IsNullOrEmpty() ? NoContent() : Ok(experiences);
            }
            [HttpGet("experiences/{id}")]
            [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoachExperienceDetailDto))]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> GetCoachExperienceDetailAsync([FromRoute] Guid id,
                CancellationToken cancellationToken) {
                var experience = await _coachExperienceService.GetCoachExperienceDetailAsync(id, cancellationToken);
                return Ok(experience);
            }

            [HttpPost("{id}/experiences")]
            [Authorize(Roles = "Admin, SuperAdmin")]
            [ValidateModel]
            [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CoachExperienceResponseDto))]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status409Conflict)]
            [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
            public async Task<IActionResult> CreateCoachExperienceAsync([FromRoute] Guid id,
                [FromBody] CoachExperienceRequestDto coachExperienceRequestDto, CancellationToken cancellationToken) {
                var createdExperience =
                    await _coachExperienceService.CreateExperienceAsync(id, coachExperienceRequestDto, cancellationToken);
                return Ok(createdExperience);

            }

            [HttpPatch("experiences/{id}")]
            [Authorize(Roles = "Admin, SuperAdmin")]
            [ValidateModel]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status409Conflict)]
            [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
            public async Task<IActionResult> UpdateCoachExperienceAsync([FromRoute] Guid id,
                [FromBody] CoachExperienceUpdateDto coachExperienceUpdateDto, CancellationToken cancellationToken) {
                await _coachExperienceService.UpdateCoachExperienceAsync(id, coachExperienceUpdateDto,
                    cancellationToken);
                return NoContent();
            }

            [HttpDelete("experiences/{id}")]
            [Authorize(Roles = "Admin, SuperAdmin")]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<IActionResult> DeleteCoachExperienceAsync([FromRoute] Guid id,
                CancellationToken cancellationToken) {
                await _coachExperienceService.DeleteCoachExperienceAsync(id, cancellationToken);
                return NoContent();
            }


        }
    }

