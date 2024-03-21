using BasketballManagerAPI.Dto.MatchDto;
using BasketballManagerAPI.Dto.StatisticDto;
using BasketballManagerAPI.Dto.TeamDto;
using BasketballManagerAPI.Dto.TicketDto;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Helpers;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfaces;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BasketballManagerAPI.Controllers {
    [ApiController]
    [Route("api/matches")]
    public class MatchController:ControllerBase {
        private readonly IMatchService _matchService;
        private readonly IStatisticService _statisticService;  
        private readonly ITicketService _ticketService;
        public MatchController(IMatchService matchService, IStatisticService statisticService, ITicketService ticketService)
        {
            _matchService = matchService;
            _statisticService = statisticService;
            _ticketService = ticketService;
        }
        [HttpGet]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<MatchResponseDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllMatchesAsync([FromQuery] MatchFiltersDto matchFiltersDto,  CancellationToken cancellationToken) {
            
            var matches = await _matchService.GetAllMatchesAsync(matchFiltersDto, cancellationToken);
            return  matches.Items.IsNullOrEmpty() ? NoContent(): Ok(matches);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MatchResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMatchAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var match = await _matchService.GetMatchAsync(id, cancellationToken);
            return Ok(match);
        }
        

        [HttpGet("{id:guid}/details")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MatchResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMatchDetailAsync([FromRoute] Guid id, CancellationToken cancellationToken) {
            var match = await _matchService.GetMatchDetailAsync(id, cancellationToken);
            return Ok(match);
        }

        [HttpGet("{id:guid}/tickets")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<TicketResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> GetAllTicketsByMatchIdAsync([FromRoute] Guid id, [FromQuery] TicketFiltersDto ticketFiltersDto,  CancellationToken cancellationToken) {
            var tickets = await _ticketService.GetAllTicketsByMatchIdAsync(id, ticketFiltersDto, cancellationToken);
            return Ok(tickets);
        }
        [HttpGet("{id:guid}/players-statistics")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PlayerStatisticDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMatchStatisticAsync([FromRoute] Guid id,
            [FromQuery] MatchStatisticFiltersDto matchStatisticFiltersDto,
            CancellationToken cancellationToken) {
            
            var statistics =
                await _statisticService.GetAllPlayersStatisticByMatchAsync(id, matchStatisticFiltersDto, cancellationToken);
                return statistics.IsNullOrEmpty() ? NoContent() : Ok(statistics); ;
            
        }
        [HttpGet("{id:guid}/teams-statistics")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MatchTeamStatisticDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllTeamsStatisticByMatchAsync([FromRoute] Guid id,
            [FromQuery] MatchStatisticFiltersDto matchStatisticFiltersDto,
            CancellationToken cancellationToken) {

            var statistics =
                await _statisticService.GetAllTeamsStatisticByMatchAsync(id, matchStatisticFiltersDto, cancellationToken);
            return statistics.IsNullOrEmpty() ? NoContent() : Ok(statistics); ;

        }
        [HttpGet("{id:guid}/total-teams-statistics")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TotalTeamStatisticDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAlGetAllTotalTeamsStatisticByMatchAsync([FromRoute] Guid id,
            [FromQuery] MatchStatisticFiltersDto matchStatisticFiltersDto,
            CancellationToken cancellationToken) {

            var statistics =
                await _statisticService.GetAllTotalTeamsStatisticByMatchAsync(id, matchStatisticFiltersDto, cancellationToken);
            return statistics.IsNullOrEmpty() ? NoContent() : Ok(statistics); ;

        }
        [HttpGet("{id:guid}/players/impacts")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TotalTeamStatisticDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CalculatePlayerImpactInMatchAsync([FromRoute] Guid id,
            [FromQuery] MatchStatisticFiltersDto matchStatisticFiltersDto,
            CancellationToken cancellationToken) {

            var statistics =
                await _statisticService.CalculatePlayerImpactInMatchAsync(id, matchStatisticFiltersDto, cancellationToken);
            return statistics.IsNullOrEmpty() ? NoContent() : Ok(statistics); ;

        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MatchResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateMatchAsync([FromBody] MatchRequestDto matchDto,
            CancellationToken cancellationToken) {
            
            var createdMatch = await _matchService.CreateMatchAsync(matchDto, cancellationToken);
            return Ok(createdMatch);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut("{id}")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateMatchAsync([FromRoute] Guid id, [FromBody] MatchRequestDto matchDto, CancellationToken cancellationToken)
        {
            await _matchService.UpdateMatchAsync(id, matchDto, cancellationToken);
            return NoContent();

        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMatchAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _matchService.DeleteMatchAsync(id,cancellationToken);
            return NoContent();
        }





    }
}
