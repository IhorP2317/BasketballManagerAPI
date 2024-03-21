using BasketballManagerAPI.Dto.OrderDto;

namespace BasketballManagerAPI.Services.Interfaces {
    public interface IOrderService {
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<OrderResponseDto> GetOrderAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> IsOrderExistAsync(Guid id, CancellationToken cancellationToken = default);
        Task<OrderResponseDto> CreateOrderAsync(OrderRequestDto orderRequestDto, CancellationToken cancellationToken = default);
        Task DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
