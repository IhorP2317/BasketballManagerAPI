namespace BasketballManagerAPI.Models {
    public class Player : Staff {
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public Position Position { get; set; }
        public int JerseyNumber { get; set; }

        public Guid? TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public ICollection<MatchHistory> MatchHistories { get; set; } = null!;
    }
}
