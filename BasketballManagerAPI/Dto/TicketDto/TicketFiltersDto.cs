using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Dto.TicketDto {
    public class TicketFiltersDto {
        [Range(0.01, Double.MaxValue, ErrorMessage = "MinPrice must be greater than 0")]
        public decimal? MinPrice { get; set; }
        [Range(0.01, Double.MaxValue, ErrorMessage = "MaxPrice must be greater than 0")]
        public decimal? MaxPrice { get; set;}
        [Range(1, int.MaxValue, ErrorMessage = "Section must be greater than 0")]
        public int? Section { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Row must be greater than 0")]
        public int? Row { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Seat must be greater than 0")]
        public int? Seat { get; set; }
        public string? SortOrder { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than 0")]
        public int PageSize { get; set; }
    }
}
