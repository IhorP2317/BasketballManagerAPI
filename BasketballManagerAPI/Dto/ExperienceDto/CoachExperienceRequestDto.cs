using BasketballManagerAPI.Helpers.ValidationAttributes;
using BasketballManagerAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Dto.ExperienceDto {
    public class CoachExperienceRequestDto: IValidatableObject {
        [NonEmptyGuid(ErrorMessage = "Team ID must be a non-empty GUID.")]
        public Guid TeamId { get; set; }
        [EnumValue(typeof(CoachStatus))]
        public string Status { get; set; } = null!;

        [Required(ErrorMessage = "Start date of birth is required!")]
        [DateTimeFormat("yyyy-MM-dd", ErrorMessage = "Date Format must be in the format yyyy-MM-dd.")]
        public string StartDate { get; set; } = null!;
        [DateTimeFormat("yyyy-MM-dd", ErrorMessage = "Date Format must be in the format yyyy-MM-dd.")]
        public string? EndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if (EndDate == null) {
                if (DateTime.TryParse(StartDate, out var startingDate)) {
                    if (startingDate > DateTime.UtcNow) {
                        yield return new ValidationResult(
                            "Start Date in outgoing experience should be in the present or past time!.",
                            new[] { nameof(EndDate) });
                    }
                }
            }
            if (DateTime.TryParse(StartDate, out var startDate) && DateTime.TryParse(EndDate, out var endDate)) {
                if (endDate <= startDate) {
                    yield return new ValidationResult(
                        "End Date must be after Start Date.",
                        new[] { nameof(EndDate) });
                }
            }
        }
    }
}
