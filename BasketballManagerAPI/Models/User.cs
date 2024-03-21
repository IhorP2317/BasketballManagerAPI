using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballManagerAPI.Models {
    public class User: BaseEntity {
        
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public decimal Balance { get; set; } 
        public string? PhotoPath { get; set; } 
        public Role Role { get; set; }
        public ICollection<Order> Orders { get; set; } = null!;
    }
}
