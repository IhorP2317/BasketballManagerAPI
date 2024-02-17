using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace BasketballManagerAPI.Models {
    public abstract class Staff:BaseEntity {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string Country { get; set; } = null!;
        public ICollection<StaffAward> StaffAwards { get; set; } = null!;
        public ICollection<StaffExperience> StaffExperiences { get; set; } = null!;

    }
}
