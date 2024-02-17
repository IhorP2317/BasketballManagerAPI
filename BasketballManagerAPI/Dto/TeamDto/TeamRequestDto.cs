using BasketballManagerAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Dto.TeamDto {
    public class TeamRequestDto:BaseEntityRequestDto {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required!")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Name can only contain letters, numbers, and spaces!")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Logo is required!")]
        public string Logo { get; set; } = null!;
    }
}
