using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Dto.PlayerDto {
    public class PlayerFiltersDto {

        public string? LastName { get; set; }
        public string? Country { get; set; }
        public string? Position { get; set; }
        public string? TeamName { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than 0")]
        public int PageSize { get; set; }
    }
}
