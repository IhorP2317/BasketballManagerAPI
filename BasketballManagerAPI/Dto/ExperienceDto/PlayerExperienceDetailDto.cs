using BasketballManagerAPI.Dto.AwardDto;
using BasketballManagerAPI.Dto.PlayerDto;
using BasketballManagerAPI.Dto.TeamDto;

namespace BasketballManagerAPI.Dto.ExperienceDto {
    public class PlayerExperienceDetailDto {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid TeamId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public ICollection<AwardResponseDto> PlayerAwards { get; set; } = null!;
        public ICollection<StatisticDto.StatisticDto> Statistics { get; set; } = null!;
    }
}
