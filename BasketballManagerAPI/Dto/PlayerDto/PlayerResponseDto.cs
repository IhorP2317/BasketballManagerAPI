using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.PlayerDto {
    public class PlayerResponseDto: BaseEntityResponseDto {
       
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string Country { get; set; } = null!;
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string Position { get; set; } = null!;
        public int JerseyNumber { get; set; }
        public string TeamName { get; set; } = null!;
        public Guid? TeamId { get; set; }
        public string PhotoUrl { get; set; } = null!;

    }
}
