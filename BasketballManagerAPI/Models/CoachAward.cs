namespace BasketballManagerAPI.Models {
    public class CoachAward {
        public Guid CoachExperienceId { get; set; }
        public CoachExperience CoachExperience { get; set; } = null!;
        public Guid AwardId { get; set; }
        public Award Award { get; set; } = null!;
    }
}
