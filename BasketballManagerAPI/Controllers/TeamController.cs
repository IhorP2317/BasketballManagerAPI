using System.Data;
using BasketballManagerAPI.Dto.TeamDto;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Interfeces;
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
            if (teams.IsNullOrEmpty())
            {
                return NoContent();
            }
            return Ok(teams);
        }
    

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeamAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {

            var team = await _teamService.GetTeamAsync(id, cancellationToken);
            if (team == null) {
                return NotFound();
            }
            return Ok(team);
        }
        [HttpGet("{id:guid}/detail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamDetailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeamDetailAsync([FromRoute] Guid id, CancellationToken cancellationToken) {

            var team = await _teamService.GetTeamDetailAsync(id, cancellationToken);
            if (team == null) {
                return NotFound();
            }
            return Ok(team);
        }

        [HttpGet("name")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TeamResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeamAsync([FromQuery] string name, CancellationToken cancellationToken) {
           

            var team = await _teamService.GetTeamAsync(name, cancellationToken);
            if (team == null) {
                return NotFound();
            }

            return Ok(team);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TeamResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> CreateTeamAsync([FromBody] TeamRequestDto teamDto, CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var createdPlayer = await _teamService.CreateTeamAsync(teamDto, cancellationToken);
            return Ok(createdPlayer);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> UpdateTeamAsync([FromRoute] Guid id, [FromBody] TeamRequestDto teamDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await _teamService.IsTeamExistsAsync(id, cancellationToken))
                return NotFound();
            await _teamService.UpdateTeamAsync(id, teamDto, cancellationToken);
            return NoContent();

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTeamAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            if(!await _teamService.IsTeamExistsAsync(id, cancellationToken))
                return NotFound();

            await _teamService.DeleteTeamAsync(id, cancellationToken);
            return NoContent();

        }


    }
}
