using System.Data;
using BasketballManagerAPI.Dto.TeamDto;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Helpers;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BasketballManagerAPI.Controllers {
    
    [ApiController]
    [Route("api/teams")]
    public class TeamController:ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TeamResponseDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllTeamsAsync(CancellationToken cancellationToken)
        {
            var teams = await _teamService.GetAllTeamsAsync(cancellationToken);
            return teams.IsNullOrEmpty() ? NoContent() : Ok(teams);
        }
        [HttpGet("filters")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<TeamResponseDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllTeamsAsync([FromQuery] TeamFiltersDto teamFiltersDto,CancellationToken cancellationToken) {
            var teams = await _teamService.GetAllTeamsWithFiltersAsync(teamFiltersDto, cancellationToken);
            return teams.Items.IsNullOrEmpty() ? NoContent() : Ok(teams);
        }


        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeamAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {

            var team = await _teamService.GetTeamAsync(id, cancellationToken);
            return Ok(team);
        }
        [HttpGet("{id:guid}/detail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamDetailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeamDetailAsync([FromRoute] Guid id, CancellationToken cancellationToken) {

            var team = await _teamService.GetTeamDetailAsync(id, cancellationToken);
            return Ok(team);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TeamResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> CreateTeamAsync([FromBody] TeamRequestDto teamDto, CancellationToken cancellationToken)
        {
            var createdPlayer = await _teamService.CreateTeamAsync(teamDto, cancellationToken);
            return Ok(createdPlayer);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut("{id}")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> UpdateTeamAsync([FromRoute] Guid id, [FromBody] TeamRequestDto teamDto, CancellationToken cancellationToken)
        {
            await _teamService.UpdateTeamAsync(id, teamDto, cancellationToken);
            return NoContent();

        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPatch("{id}/avatar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTeamAvatarAsync([FromRoute] Guid id,
            [FromForm] IFormFile picture,
            CancellationToken cancellationToken) {
            await _teamService.UpdateTeamAvatarAsync(id, picture, cancellationToken);
            return NoContent();

        }

        [HttpGet("{id}/avatar")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadTeamAvatarAsync([FromRoute] Guid id,
            CancellationToken cancellationToken) {
            var fileDto = await _teamService.DownloadTeamAvatarAsync(id, cancellationToken);
            return File(fileDto.Content, fileDto.MimeType, fileDto.FileName);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTeamAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _teamService.DeleteTeamAsync(id, cancellationToken);
            return NoContent();

        }


    }
}
