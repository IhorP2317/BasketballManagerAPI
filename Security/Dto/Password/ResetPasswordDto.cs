using System.ComponentModel.DataAnnotations;
using Security.Helpers.ValidationAttributes;

namespace Security.Dto.PasswordDto {
    public class ResetPasswordDto {
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserId is required")]
        public string UserId { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Token is required")]
        public string Token { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        [PasswordPolicy]
        public string NewPassword { get; set; } = null!;
    }
}
