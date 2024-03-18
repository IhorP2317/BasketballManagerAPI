using System.ComponentModel.DataAnnotations;

namespace Security.Dto.PasswordDto {
    public class ForgotPasswordDto {
        [Required(ErrorMessage = "The email field is required")]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
