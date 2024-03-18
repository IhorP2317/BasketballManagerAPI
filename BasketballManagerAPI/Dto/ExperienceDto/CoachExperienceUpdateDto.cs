using BasketballManagerAPI.Helpers.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Dto.ExperienceDto {
    public class CoachExperienceUpdateDto {
        [Required(AllowEmptyStrings = false, ErrorMessage = "End date is required!")]
        [DateTimeFormat("yyyy-MM-dd", ErrorMessage = "Date Format must be in the format yyyy-MM-dd.")]
        public string? EndDate { get; set; }
    }
}
