using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Dto.ExperienceDto;
using BasketballManagerAPI.Dto.TeamDto;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.PlayerDto {
    public class PlayerDetailDto:BaseEntityResponseDto {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string Country { get; set; } = null!;
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string Position { get; set; } = null!;
        public int JerseyNumber { get; set; }
        public Guid? TeamId { get; set; }
        public TeamResponseDto Team { get; set; } = null!;
        public string? PhotoPath { get; set; }
        public ICollection<PlayerExperienceDetailDto> PlayerExperiences { get; set; } = null!;

    }
}
