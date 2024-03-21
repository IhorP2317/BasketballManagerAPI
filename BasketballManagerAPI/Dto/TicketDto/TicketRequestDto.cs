using System.ComponentModel.DataAnnotations;
using BasketballManagerAPI.Helpers.ValidationAttributes;

namespace BasketballManagerAPI.Dto.TicketDto {
    public class TicketRequestDto {
        [NonEmptyGuid(ErrorMessage = "Match id must not be empty!")]
        public Guid MatchId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Section is required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Section must be in range between 1 and 10!")]
        public int? Section { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Row is required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Row must be in range between 1 and 10!")]
        public int? Row { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Seat is required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Seat must be in range between 1 and 10!")]
        public int? Seat { get; set; } = null!;
    }
}
