using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.ExperienceDto {
    public class CoachExperienceResponseDto {
        public Guid Id { get; set; }
        public Guid CoachId { get; set; }
        public Guid TeamId { get; set; }
        public string Status { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
