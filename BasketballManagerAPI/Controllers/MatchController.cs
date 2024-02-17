using BasketballManagerAPI.Dto.MatchDto;
using BasketballManagerAPI.Dto.StatisticDto;
using BasketballManagerAPI.Dto.TeamDto;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Helpers;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BasketballManagerAPI.Controllers {
    [ApiController]
    [Route("api/matches")]
    public class MatchController:ControllerBase {
        private readonly IMatchService _matchService;
        private readonly IStatisticService _statisticService;   
        public MatchController(IMatchService matchService, IStatisticService statisticService)
        {
            _matchService = matchService;
            _statisticService = statisticService;
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
        [HttpGet("{id:guid}/statistics")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PlayerStatisticDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMatchStatisticAsync([FromRoute] Guid id,
            [FromQuery] MatchStatisticFiltersDto matchStatisticFiltersDto,
            CancellationToken cancellationToken) {
            
            var statistics =
                await _statisticService.GetAllMatchStatisticAsync(id, matchStatisticFiltersDto, cancellationToken);
                return statistics.IsNullOrEmpty() ? NoContent() : Ok(statistics); ;
            
        }

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
