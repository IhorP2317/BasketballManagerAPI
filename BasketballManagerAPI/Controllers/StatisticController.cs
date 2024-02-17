using BasketballManagerAPI.Dto.MatchDto;
using BasketballManagerAPI.Dto.StatisticDto;
using BasketballManagerAPI.Models;
using BasketballManagerAPI.Services.Implementations;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BasketballManagerAPI.Controllers {
    [ApiController]
    [Route("api/statistics/")]
    public class StatisticController:ControllerBase {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }


        [HttpGet("matches/{matchId:guid}/players/{playerId:guid}/{timeUnit:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StatisticDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStatisticAsync([FromRoute] Guid matchId, [FromRoute] Guid playerId,
            [FromRoute] int timeUnit, CancellationToken cancellationToken)
        {
            var statistic = await _statisticService.GetStatisticAsync(playerId, matchId, timeUnit, cancellationToken);
            if (statistic == null)
            {
                return NotFound();
            }
            return Ok(statistic);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StatisticDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> CreateStatisticAsync([FromBody] StatisticDto statisticDto,
            CancellationToken cancellationToken) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var createdStatistic = await _statisticService.CreateStatisticAsync(statisticDto, cancellationToken);
            return Ok(createdStatistic);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> UpdateStatisticAsync([FromBody] StatisticDto statisticDto,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (!await _statisticService.IsStatisticExistAsync(statisticDto.MatchId, statisticDto.PlayerId, statisticDto.TimeUnit.GetValueOrDefault(), cancellationToken))
                return NotFound();
            await _statisticService.UpdateStatisticAsync(statisticDto, cancellationToken);
            return NoContent();
        }
        [HttpDelete("matches/{matchId:guid}/players/{playerId:guid}/{timeUnit:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStatisticAsync([FromRoute] Guid matchId, [FromRoute] Guid playerId,
        [FromRoute] int timeUnit, CancellationToken cancellationToken) {
            if (!await _statisticService.IsStatisticExistAsync(matchId, playerId, timeUnit, cancellationToken))
                return NotFound();
            await _statisticService.DeleteStatisticAsync(playerId, matchId, timeUnit, cancellationToken);
            return NoContent();
        }
    }
}
