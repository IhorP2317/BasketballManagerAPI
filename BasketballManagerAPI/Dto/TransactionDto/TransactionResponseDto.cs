using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.TransactionDto {
    public class TransactionResponseDto: BaseEntityResponseDto {
        public decimal Value { get; set; }
        public Guid UserId { get; set; }
        public string Status { get; set; } = null!;
    }
}
