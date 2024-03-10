using BasketballManagerAPI.Models;
using System.ComponentModel.DataAnnotations;
using BasketballManagerAPI.Helpers.ValidationAttributes;

namespace BasketballManagerAPI.Dto.UserDto {
    public class UserRequestDto {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required!")]
        public string LastName { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required!")]
        public string FirstName { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Address is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone number is required!")]
        [Phone(ErrorMessage = "Invalid Phone number!")]
        public string PhoneNumber { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Photo is required!")]
        public string PhotoUrl { get; set; } = null!;
        [EnumValue(typeof(Role))]
        public string Role { get; set; } = null!;
        
    }
}
