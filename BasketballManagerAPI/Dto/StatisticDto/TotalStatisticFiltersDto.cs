using BasketballManagerAPI.Helpers.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Dto.StatisticDto {
    public class TotalStatisticFiltersDto {
        [Range(1892, int.MaxValue, ErrorMessage = "Year should be earlier than 1892")]
        public int? Year { get; set; }
    }
}
