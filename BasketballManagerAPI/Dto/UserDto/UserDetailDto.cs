using BasketballManagerAPI.Dto.OrderDto;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.UserDto {
    public class UserDetailDto:BaseEntityResponseDto {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public decimal Balance { get; set; }
        public string PhotoPath { get; set; } = null!;
        public string Role { get; set; } = null!;
        public ICollection<OrderResponseDto> Orders { get; set; } = null!;
    }
}
