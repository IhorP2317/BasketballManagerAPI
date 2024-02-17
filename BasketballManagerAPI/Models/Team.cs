using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballManagerAPI.Models {
    public class Team:BaseEntity {
        public string Name { get; set; } = null!;
        public Guid? CoachId { get; set; }
        public Coach Coach { get; set; } = null!;
        public ICollection<Player> Players { get; set; } = null!;
        public ICollection<StaffExperience> StaffExperiences { get; set; } = null!;
        public Match Match { get; set; } = null!;

    }
}
