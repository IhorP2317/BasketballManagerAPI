namespace BasketballManagerAPI.Models {
    public class PlayerExperience:BaseEntity {
        public Guid PlayerId { get; set; }
        public Player Player { get; set; } = null!;
        public Guid TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
