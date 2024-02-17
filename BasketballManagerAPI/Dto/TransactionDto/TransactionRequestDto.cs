using BasketballManagerAPI.Helpers.ValidationAttributes;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.TransactionDto {
    public class TransactionRequestDto:BaseEntityRequestDto {
        public decimal Value { get; set; }
        [EnumValue(typeof(TransactionStatus))]
        public string Status { get; set; } = null!;
      
    }
}
