namespace BasketballManagerAPI.Models {
    public class StaffAward {
        public Guid StaffId { get; set; }
        public Staff Staff { get; set; } = null!;
        public Guid AwardId { get; set; } 
        public Award Award { get; set; } = null!;
    }
}
