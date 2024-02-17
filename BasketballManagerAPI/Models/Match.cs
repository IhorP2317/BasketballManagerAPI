using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballManagerAPI.Models {
    public class Match:BaseEntity {
        public string Location { get; set; } = null!;
        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid HomeTeamId { get; set; }
        public Team HomeTeam { get; set; } = null!;

        public Guid AwayTeamId { get; set; }
        public Team AwayTeam { get; set; } = null!;
        public ICollection<MatchHistory> MatchHistories { get; set; } = null!;

    }
}
