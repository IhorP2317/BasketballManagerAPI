using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballManagerAPI.Models {
    public class Award:BaseEntity
    {
        public string Name { get; set; } = null!;
        public DateOnly Date { get; set; }

        public ICollection<StaffAward> StaffAwards { get; set; } = null!;
    }
}
