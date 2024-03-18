namespace BasketballManagerAPI.Models {
    public class CoachExperience:BaseEntity {
        public Guid CoachId { get; set; }
        public Coach Coach { get; set; } = null!;
        public Guid TeamId { get; set; }
        public CoachStatus Status { get; set; }
        public Team Team { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public ICollection<CoachAward> CoachAwards { get; set; } = null!;
    }
}
