namespace BasketballManagerAPI.Models {
    public class Ticket:BaseEntity {
        public Guid MatchId { get; set; }
        public Match Match { get; set; } = null!;
        public string Section { get; set; } = null!;
        public int Row { get; set; }
        public int Seat { get; set; }
        public Guid? TransactionId { get; set; }
        public Transaction Transaction { get; set; } = null!;
        public decimal Price { get; set; }

    }
}
