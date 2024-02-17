using System.ComponentModel.DataAnnotations;
using BasketballManagerAPI.Helpers.ValidationAttributes;

namespace BasketballManagerAPI.Dto.StatisticDto {
    public class MatchStatisticFiltersDto {
        
        public Guid? TeamId { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Time unit must be a positive number!")]
        public int? TimeUnit { get; set; }
        [Required(ErrorMessage = "IsAccumulativeDisplayEnabled is required!")]
        public bool? IsAccumulativeDisplayEnabled { get; set; }
    }
}
