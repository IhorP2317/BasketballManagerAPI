namespace BasketballManagerAPI.Models {
    public class PlayerAward {
        public Guid PlayerExperienceId { get; set; }
        public PlayerExperience PlayerExperience { get; set; } = null!;
        public Guid AwardId { get; set; }
        public Award Award { get; set; } = null!;
    }
}
