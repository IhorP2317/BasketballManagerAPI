using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballManagerAPI.Models {
    public class Seat:BaseEntity {
        public string Name { get; set; } = null!;
        public bool IsFree { get; set; } 
    }
}
