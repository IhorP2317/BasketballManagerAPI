using BasketballManagerAPI.Helpers.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Dto.ExperienceDto {
    public class PlayerExperienceFiltersDto {
        public Guid? TeamId { get; set; }
       
        [DateTimeFormat("yyyy-MM-dd HH:mm:ss", ErrorMessage = "DateTime must be in the format yyyy-MM-dd HH:mm:ss.")]
        public string? MatchStartTime { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than 0")]
        public int PageSize { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
    }
}
