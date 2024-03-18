using System.ComponentModel.DataAnnotations;
using BasketballManagerAPI.Helpers.ValidationAttributes;

namespace BasketballManagerAPI.Dto.ExperienceDto {
    public class PlayerExperienceUpdateDto {
        [Required(AllowEmptyStrings = false, ErrorMessage ="End date is required!")]
        [DateTimeFormat("yyyy-MM-dd", ErrorMessage = "Date Format must be in the format yyyy-MM-dd.")]
        public string? EndDate { get; set; }
    }
}
