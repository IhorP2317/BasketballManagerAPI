using BasketballManagerAPI.Dto.TicketDto;
using BasketballManagerAPI.Helpers.ValidationAttributes;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.OrderDto {
    public class OrderRequestDto {
        [NonEmptyGuid(ErrorMessage = "User Id must not be empty!")]
        public Guid UserId { get; set; }

        public ICollection<TicketRequestDto> Tickets { get; set; } = null!;


    }
}
