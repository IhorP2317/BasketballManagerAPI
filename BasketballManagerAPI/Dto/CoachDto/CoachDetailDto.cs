using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Dto.ExperienceDto;
using BasketballManagerAPI.Dto.PlayerExperienceDto;
using BasketballManagerAPI.Dto.TeamDto;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.CoachDto {
    public class CoachDetailDto:BaseEntityResponseDto {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string Country { get; set; } = null!;
        public Guid? TeamId { get; set; }

        public string CoachStatus { get; set; } = null!;
        public string Specialty { get; set; } = null!;
        public string PhotoUrl { get; set; } = null!;
        public ICollection<AwardResponseDto> CoachAwards { get; set; } = null!;
        public ICollection<StaffExperienceResponseDto> CoachExperiences { get; set; } = null!;
    }
}
