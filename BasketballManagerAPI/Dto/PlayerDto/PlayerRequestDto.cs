using BasketballManagerAPI.Models;
using System.ComponentModel.DataAnnotations;
using BasketballManagerAPI.Helpers.ValidationAttributes;

namespace BasketballManagerAPI.Dto.PlayerDto {
    public class PlayerRequestDto: BaseEntityRequestDto {
        [Required(AllowEmptyStrings = false,ErrorMessage = "Last name is required!")]
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
        [Required(ErrorMessage = "Height is required!")]
        [Range(1.01,double.MaxValue, ErrorMessage = "Height must be greater then 1 meter!")]
        public decimal Height { get; set; }
        [Required(ErrorMessage = "Weight is required!")]
        [Range(50.01, double.MaxValue, ErrorMessage = "Weight must be greater then 50 kg!")]
        public decimal Weight { get; set; }
        public Guid? TeamId { get; set; }
        [EnumValue(typeof(Position))]
        public string Position { get; set; } = null!;
        [Range(0, int.MaxValue, ErrorMessage = "Jersey Number must be positive number!")]
        public int JerseyNumber { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Photo is required!")]
        public string PhotoUrl { get; set; } = null!;
    }
}
