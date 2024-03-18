using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballManagerAPI.Models {
    public class Award:BaseEntity
    {
        public string Name { get; set; } = null!;
        public DateOnly Date { get; set; }
        public bool IsIndividualAward { get; set; }
        public string? PhotoPath { get; set; }

        public ICollection<CoachAward> CoachAwards { get; set; } = null!;
        public ICollection<PlayerAward> PlayerAwards { get; set; } = null!;
    }
}
