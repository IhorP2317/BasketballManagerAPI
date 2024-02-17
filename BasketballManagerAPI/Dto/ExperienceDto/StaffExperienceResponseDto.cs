using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.PlayerExperienceDto {
    public class StaffExperienceResponseDto:BaseEntityResponseDto {
        public Guid StaffId { get; set; }
        public Guid TeamId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
