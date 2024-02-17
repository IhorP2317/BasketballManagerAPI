using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.TicketDto {
    public class TicketResponseDto:BaseEntityResponseDto {
        public Guid MatchId { get; set; }
        public string Section { get; set; } = null!;
        public int Row { get; set; }
        public int Seat { get; set; }
        public Guid? TransactionId { get; set; }
        public decimal Price { get; set; }
    }
}
