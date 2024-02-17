namespace BasketballManagerAPI.Models {
    public class Player : BaseEntity {

        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string Country { get; set; } = null!;
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public Position Position { get; set; }
        public int JerseyNumber { get; set; }
        public Guid? TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public string PhotoUrl { get; set; } = null!;
        public ICollection<Statistic> Statistics { get; set; } = null!;
        public ICollection<PlayerAward> PlayerAwards { get; set; } = null!;
        public ICollection<PlayerExperience> PlayerExperiences { get; set; } = null!;

    }
}
