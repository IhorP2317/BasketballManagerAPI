using System.ComponentModel.DataAnnotations;

namespace Security.Dto {
    public class TokenDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Access Token is Required!")]
        public string AccessToken { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Access Token is Required!")]
        public string RefreshToken { get; set; } = null!;
    }
}
