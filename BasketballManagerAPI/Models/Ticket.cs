namespace BasketballManagerAPI.Models {
    public class Ticket:BaseEntity {
        public Guid MatchId { get; set; }
        public Match Match { get; set; } = null!;
        public int Section { get; set; } 
        public int Row { get; set; }
        public int Seat { get; set; }
        public Guid? OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public decimal Price { get; set; }

    }
}
