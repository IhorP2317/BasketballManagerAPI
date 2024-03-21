using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.TicketDto {
    public class TicketResponseDto:BaseEntityResponseDto {
        public Guid MatchId { get; set; }
        public int Section { get; set; } 
        public int Row { get; set; }
        public int Seat { get; set; }
        public Guid? OrderId { get; set; }
        public decimal Price { get; set; }
    }
}
