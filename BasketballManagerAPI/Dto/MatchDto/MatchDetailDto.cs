
using BasketballManagerAPI.Dto.TeamDto;
using BasketballManagerAPI.Dto.TicketDto;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.MatchDto {
    public class MatchDetailDto:BaseEntityResponseDto {
        public string Location { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid HomeTeamId { get; set; }
        public TeamResponseDto HomeTeam { get; set; } = null!;
        public int? HomeTeamScore { get; set; }
        public int? AwayMatchScore { get; set; }
        public string Status { get; set; } = null!;

        public Guid AwayTeamId { get; set; }
        public TeamResponseDto AwayTeam { get; set; } = null!;
        public ICollection<StatisticDto.StatisticDto> Statistics { get; set; } = null!;
        public ICollection<TicketResponseDto> Tickets { get; set; } = null!;
    }
}
