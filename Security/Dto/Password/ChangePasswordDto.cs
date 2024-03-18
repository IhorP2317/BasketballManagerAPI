using System.ComponentModel.DataAnnotations;
using Security.Helpers.ValidationAttributes;

namespace Security.Dto.PasswordDto {
    public class ChangePasswordDto {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "NewPassword is required")]
        [PasswordPolicy]
        public string NewPassword { get; set; } = null!;
    }
}
