using System.ComponentModel.DataAnnotations;
using BasketballManagerAPI.Helpers.ValidationAttributes;

namespace BasketballManagerAPI.Dto.TicketDto {
    public class TicketRequestDto {
        [NonEmptyGuid(ErrorMessage = "Match id must not be empty!")]
        public Guid MatchId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Section is required!")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Section can only contain letters, numbers, and spaces!")]
        public string Section { get; set; } = null!;
        [Range(1,10, ErrorMessage = "Row must be in range between 1 and 10!")]
        public int Row { get; set; }
        [Range(1, 10, ErrorMessage = "Seat must be in range between 1 and 10!")]
        public int Seat { get; set; }
        public Guid? TransactionId { get; set; }
        [Range(0.01, Double.MaxValue, ErrorMessage = "Price must be positive!")]
        public decimal Price { get; set; }
    }
}
