using System.ComponentModel.DataAnnotations;
using BasketballManagerAPI.Helpers.ValidationAttributes;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.UserDto {
    public class UserFiltersDto {
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Last Name can only contain letters, numbers, and spaces!")]
        public string? LastName { get; set; }
        
        public string? Role { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than 0")]
        public int PageSize { get; set; }
    }
}
