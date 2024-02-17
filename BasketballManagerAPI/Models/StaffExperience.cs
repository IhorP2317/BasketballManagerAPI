namespace BasketballManagerAPI.Models {
    public class StaffExperience:BaseEntity {
        public Guid StaffId { get; set; }
        public Staff Staff { get; set; } = null!;
        public Guid TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set;}
    }
}
