using BasketballManagerAPI.Dto.OrderDto;
using BasketballManagerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasketballManagerAPI.Controllers {
    [ApiController]
    [Route("api/orders")]
    public class OrderController:ControllerBase {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetOrderAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderAsync(id, cancellationToken);
            return Ok(order);
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderRequestDto orderRequestDto,
            CancellationToken cancellationToken)
        {
            var createdOrder = await _orderService.CreateOrderAsync(orderRequestDto, cancellationToken);
            return Ok(createdOrder);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> DeleteOrderAsync([FromRoute] Guid id, CancellationToken cancellation)
        {
            await _orderService.DeleteOrderAsync(id, cancellation);
            return NoContent();
        }

    }
}
