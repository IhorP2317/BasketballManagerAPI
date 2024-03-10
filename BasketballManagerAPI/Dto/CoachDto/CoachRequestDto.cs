using BasketballManagerAPI.Helpers.ValidationAttributes;
using BasketballManagerAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Dto.CoachDto {
    public class CoachRequestDto {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required!")]
        public string LastName { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required!")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Date of birth is required!")]
        [DateTimeFormat("yyyy-MM-dd", ErrorMessage = "Date Format must be in the format yyyy-MM-dd.")]
        [MinimumAge(18, ErrorMessage = "Age must be at least 18 years!")]
        public string DateOfBirth { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Country is required!")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Country can only contain letters, numbers, and spaces!")]
        public string Country { get; set; } = null!;

        public Guid? TeamId { get; set; }
        [EnumValue(typeof(CoachStatus))]
        public string CoachStatus { get; set; } = null!;
        [EnumValue(typeof(Specialty))]
        public string Specialty { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Photo is required!")]
        public string PhotoUrl { get; set; } = null!;

    }
}
