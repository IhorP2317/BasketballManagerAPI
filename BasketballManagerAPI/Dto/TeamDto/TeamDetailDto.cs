using BasketballManagerAPI.Dto.CoachDto;
using BasketballManagerAPI.Dto.ExperienceDto;
using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Dto.PlayerExperienceDto;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.TeamDto {
    public class TeamDetailDto {
        public string Name { get; set; } = null!;
        public string Logo { get; set; } = null!;
        public ICollection<PlayerResponseDto> Players { get; set; } = null!;
        public ICollection<StaffExperienceResponseDto> PlayerExperiences { get; set; } = null!;
        public ICollection<StaffExperienceResponseDto> CoachExperiences { get; set; } = null!;
        public ICollection<CoachResponseDto> Coaches { get; set; } = null!;
    }
}
