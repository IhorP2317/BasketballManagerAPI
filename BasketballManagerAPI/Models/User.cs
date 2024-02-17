using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballManagerAPI.Models {
    public class User: BaseEntity {
        
        public string LastName { get; set; } = null!;
        public string FistName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public decimal Balance { get; set; } 
        public string PhoneNumber { get; set; } = null!;
        public string PhotoUrl { get; set; } = null!;
        public Role Role { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = null!;
    }
}
