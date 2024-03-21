using BasketballManagerAPI.Dto.MatchDto;
using BasketballManagerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BasketballManagerAPI.Controllers {
    [ApiController]
    [Route("api/tickets")]
    public class TicketController:ControllerBase {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MatchResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTicketAsync([FromRoute] Guid id, CancellationToken cancellationToken) {
            var ticket = await _ticketService.GetTicketAsync(id, cancellationToken);
            return Ok(ticket);
        }

    }
}
