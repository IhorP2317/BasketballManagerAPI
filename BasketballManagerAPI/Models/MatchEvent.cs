namespace BasketballManagerAPI.Models {
    public class MatchEvent {
        public MatchEventId Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<MatchHistory> MatchHistories { get; set; } = null!;

    }
}
