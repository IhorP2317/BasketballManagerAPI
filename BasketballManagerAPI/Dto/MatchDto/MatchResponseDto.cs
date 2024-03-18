using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.MatchDto {
    public class MatchResponseDto: BaseEntityResponseDto{
            public string Location { get; set; } = null!;
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string HomeTeamName { get; set; } = null!;
            
            public string AwayTeamName { get; set; } = null!;
            public int? HomeTeamScore { get; set; }
            public int? AwayTeamScore { get;set; }
            public string Status { get; set; } = null!;
        
    }
}
