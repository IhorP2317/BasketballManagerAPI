using System.Data;
using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Dto.StatisticDto;
using BasketballManagerAPI.Filters;
using BasketballManagerAPI.Helpers;
using BasketballManagerAPI.Services.Interfeces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BasketballManagerAPI.Controllers {
    [ApiController]
    [Route("api/players")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly IStatisticService _statisticService;

        public PlayerController(IPlayerService playerService, IStatisticService statisticService)
        {
            _playerService = playerService;
            _statisticService = statisticService;
        }

        [HttpPost("get")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<PlayerResponseDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllPlayersAsync([FromBody] PlayerFiltersDto playerFiltersDto, CancellationToken cancellationToken)
        {
            
            var players = await _playerService.GetAllPlayersAsync(playerFiltersDto, cancellationToken);
            return players.Items.IsNullOrEmpty()? NoContent() : Ok(players);
        }

       

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlayerResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlayerByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var player = await _playerService.GetPlayerByIdAsync(id, cancellationToken);
            return Ok(player);
        }

        [HttpGet("{id:guid}/details")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlayerResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlayerByIdDetailAsync([FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var player = await _playerService.GetPlayerByIdDetailAsync(id, cancellationToken);
            return Ok(player);
        }
        [HttpGet("{id:guid}/statistics")]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TotalAnnuallyStatisticDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> GetAllAnnuallyStatisticAsync([FromRoute] Guid id, [FromQuery] TotalStatisticFiltersDto statisticFiltersDto,
            CancellationToken cancellationToken) {
         
           var statistics = await _statisticService.GetAllAnnuallyStatisticAsync(id, statisticFiltersDto, cancellationToken);
           if (statistics.IsNullOrEmpty())
               return NoContent();
           return Ok(statistics);

        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PlayerResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> CreatePlayerAsync([FromBody] PlayerRequestDto playerDto,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPlayer = await _playerService.CreatePlayerAsync(playerDto, cancellationToken);
            return Ok(createdPlayer);
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> UpdatePlayerAsync([FromRoute] Guid id, PlayerRequestDto playerDto, CancellationToken cancellationToken) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (!await _playerService.IsPlayerExistAsync(id, cancellationToken))
                return NotFound();
            await _playerService.UpdatePlayerAsync(id, playerDto, cancellationToken);
            return NoContent();
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePlayerAsync([FromRoute] Guid id, CancellationToken cancellationToken) {

            if (!await _playerService.IsPlayerExistAsync(id, cancellationToken))
                return NotFound();
            await _playerService.DeletePlayerAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
