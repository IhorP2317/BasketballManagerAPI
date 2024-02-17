using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasketballManagerAPI.Models {
    public class CoachStatus {
        public CoachStatusId Id { get; set; }

        public string Name { get; set; } = null!;
        public ICollection<Coach> Coaches { get; set; } = null!;
    }
}
