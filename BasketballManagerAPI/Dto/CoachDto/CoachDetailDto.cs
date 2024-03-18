using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Dto.ExperienceDto;
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
        public string? PhotoPath { get; set; } 
        public ICollection<CoachExperienceDetailDto> CoachExperiences { get; set; } = null!;
    }
}
