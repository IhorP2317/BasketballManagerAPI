using BasketballManagerAPI.Dto.TransactionDto;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.UserDto {
    public class UserDetailDto:BaseEntityResponseDto {
        public string LastName { get; set; } = null!;
        public string FistName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public decimal Balance { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string PhotoUrl { get; set; } = null!;
        public string Role { get; set; } = null!;
        public ICollection<TransactionResponseDto> Transactions { get; set; } = null!;
    }
}
