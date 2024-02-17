using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballManagerAPI.Models {
    public class Team:BaseEntity {
        public string Name { get; set; } = null!;
        public string Logo { get; set; } = null!;
        public ICollection<Player> Players { get; set; } = null!;
        public ICollection<PlayerExperience> PlayerExperiences { get; set; } = null!;
        public ICollection<Coach> Coaches { get; set; } = null!;
        public ICollection<CoachExperience> CoachExperiences { get; set; } = null!;

    }
}
