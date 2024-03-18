namespace BasketballManagerAPI.Models {
    public class Coach:BaseEntity {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string Country { get; set; } = null!;
        public Guid? TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public string? PhotoPath { get; set; }
        public CoachStatus CoachStatus { get; set; }
        public Specialty Specialty { get; set; }
        public ICollection<CoachExperience> CoachExperiences { get; set; } = null!;


    }
}
