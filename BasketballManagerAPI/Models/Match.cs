using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballManagerAPI.Models {
    public class Match:BaseEntity {
        public string Location { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public MatchStatus Status { get; set; }
        public Guid HomeTeamId { get; set; }
        public Team HomeTeam { get; set; } = null!;
        public int SectionCount { get; set; }
        public int RowCount { get; set; }
        public int SeatCount { get; set; }
        public Guid AwayTeamId { get; set; }
        public Team AwayTeam { get; set; } = null!;
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
        public ICollection<Statistic> Statistics { get; set; } = null!;
        public ICollection<Ticket> Tickets { get; set; } = null!;
    }
}
