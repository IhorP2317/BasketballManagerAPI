namespace BasketballManagerAPI.Models {
    public class CoachAward {
        public Guid CoachId { get; set; }
        public Coach Coach { get; set; } = null!;
        public Guid AwardId { get; set; }
        public Award Award { get; set; } = null!;
    }
}
