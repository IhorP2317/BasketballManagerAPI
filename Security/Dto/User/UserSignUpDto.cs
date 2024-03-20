using Security.Helpers.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Security.Dto.UserDto
{
    public class UserSignUpDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required!")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Last Name can only contain letters, numbers, and spaces!")]
        public string LastName { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required!")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "First Name can only contain letters, numbers, and spaces!")]
        public string FirstName { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Address is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]

        [PasswordPolicy]
        public string Password { get; set; } = null!;



    }
}
