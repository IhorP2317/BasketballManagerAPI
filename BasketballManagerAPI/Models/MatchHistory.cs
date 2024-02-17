using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballManagerAPI.Models {
    public class MatchHistory: BaseEntity{
        
        public  MatchEventId MatchEventId { get; set; }
        public MatchEvent MatchEvent { get; set; } = null!;
        public Guid MatchId { get; set; }
        public Match Match { get; set; } = null!;
        public Guid PlayerId { get; set; }
        public Player Player { get; set; } = null!;
        public TimeOnly Time { get; set; }

    }
}
