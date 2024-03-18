using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Dto.CoachDto;
using BasketballManagerAPI.Dto.TeamDto;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.ExperienceDto {
    public class CoachExperienceDetailDto {
        public Guid Id { get; set; }
        public Guid CoachId { get; set; }
        public Guid TeamId { get; set; }
        public string Status { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public ICollection<AwardResponseDto> CoachAwards { get; set; } = null!;
    }
}

