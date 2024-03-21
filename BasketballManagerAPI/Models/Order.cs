namespace BasketballManagerAPI.Models {
    public class Order: BaseEntity {
        public decimal TotalPrice { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<Ticket> Tickets { get; set; } = null!;


    }
}
