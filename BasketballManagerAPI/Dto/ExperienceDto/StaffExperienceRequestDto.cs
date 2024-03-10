using BasketballManagerAPI.Helpers.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Dto.ExperienceDto {
    public class StaffExperienceRequestDto {
        [NonEmptyGuid(ErrorMessage = "Staff ID must be a non-empty GUID.")]
        public Guid StaffId { get; set; }
        [NonEmptyGuid(ErrorMessage = "Team ID must be a non-empty GUID.")]
        public Guid TeamId { get; set; }
        [Required(ErrorMessage = "Start date of birth is required!")]
        [DateTimeFormat("yyyy-MM-dd", ErrorMessage = "Date Format must be in the format yyyy-MM-dd.")]
        public DateOnly? StartDate { get; set; }
        [DateTimeFormat("yyyy-MM-dd", ErrorMessage = "Date Format must be in the format yyyy-MM-dd.")]
        public DateOnly? EndDate { get; set; }
    }
}
