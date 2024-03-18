using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Dto.TeamDto {
    public class TeamFiltersDto {
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Name can only contain letters, numbers, and spaces!")]
        public string? Name { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than 0")]
        public int PageSize { get; set; }

    }
}
