using BasketballManagerAPI.Dto.CoachDto;
using BasketballManagerAPI.Dto.ExperienceDto;
using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.TeamDto {
    public class TeamDetailDto {
        public string Name { get; set; } = null!;
        public string? PhotoPath { get; set; }
        public ICollection<PlayerResponseDto> Players { get; set; } = null!;
        public ICollection<PlayerExperienceResponseDto> PlayerExperiences { get; set; } = null!;
        public ICollection<CoachExperienceResponseDto> CoachExperiences { get; set; } = null!;
        public ICollection<CoachResponseDto> Coaches { get; set; } = null!;
    }
}
