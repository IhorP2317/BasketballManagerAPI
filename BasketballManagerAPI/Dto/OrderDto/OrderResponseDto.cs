using BasketballManagerAPI.Dto.TicketDto;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.OrderDto
{
    public class OrderResponseDto : BaseEntityResponseDto
    {
        public decimal TotalPrice { get; set; }
        public Guid UserId { get; set; }
        public ICollection<TicketResponseDto> Tickets { get; set; } = null!;
    }
}
