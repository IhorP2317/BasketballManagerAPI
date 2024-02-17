namespace BasketballManagerAPI.Models {
    public class PlayerAward {
        public Guid PlayerId { get; set; }
        public Player Player { get; set; } = null!;
        public Guid AwardId { get; set; }
        public Award Award { get; set; } = null!;
    }
}
