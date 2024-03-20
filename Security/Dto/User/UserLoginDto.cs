using Security.Helpers.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Security.Dto.UserDto
{
    public class UserLoginDto
    {


        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Address is required!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]

        [PasswordPolicy]
        public string Password { get; set; } = null!;
    }
}
