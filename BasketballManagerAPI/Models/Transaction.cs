namespace BasketballManagerAPI.Models {
    public class Transaction: BaseEntity {
        public decimal Value { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public TransactionStatus Status { get; set; }
        public ICollection<Ticket> Tickets { get; set; } = null!;


    }
}
