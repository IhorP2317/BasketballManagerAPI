using BasketballManagerAPI.Helpers.ValidationAttributes;
using BasketballManagerAPI.Models;
using System.ComponentModel.DataAnnotations;
using BasketballManagerAPI.Dto.ExperienceDto;
using Microsoft.IdentityModel.Tokens;

namespace BasketballManagerAPI.Dto.CoachDto {
    public class CoachRequestDto:IValidatableObject{
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required!")]
        public string LastName { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required!")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Date of birth is required!")]
        [DateTimeFormat("yyyy-MM-dd", ErrorMessage = "Date Format must be in the format yyyy-MM-dd.")]
        [MinimumAge(18, ErrorMessage = "Age must be at least 18 years!")]
        public string DateOfBirth { get; set; } = null!;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Country is required!")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Country can only contain letters, numbers, and spaces!")]
        public string Country { get; set; } = null!;

        public Guid? TeamId { get; set; }
        [EnumValue(typeof(CoachStatus))]
        public string CoachStatus { get; set; } = null!;
        [EnumValue(typeof(Specialty))]
        public string Specialty { get; set; } = null!;
        public ICollection<CoachExperienceRequestDto>? CoachExperiences { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if (!CoachExperiences.IsNullOrEmpty()) {
                if (DateTime.TryParse(DateOfBirth, out var birthDay)) {
                    var coach18thBirthday = birthDay.AddYears(18);
                    if (CoachExperiences.Any(c => (DateTime.TryParse(c.StartDate, out var startDate) && startDate < coach18thBirthday))) {
                        yield return new ValidationResult(
                            "There can no be experiences that older then start professional career of player!",
                            new[] { nameof(CoachExperiences) });
                    }

                }

                int ongoingExperiencesCount = CoachExperiences.Count(pe => pe.EndDate == null);
                if (ongoingExperiencesCount > 1) {
                    yield return new ValidationResult(
                        "There can be at most one ongoing experience!",
                        new[] { nameof(CoachExperiences) });
                }


                var sortedExperiences = CoachExperiences
                    .OrderBy(pe => pe.StartDate)
                    .ToList();
                if (sortedExperiences.LastOrDefault().EndDate != null && ongoingExperiencesCount == 1) {

                    yield return new ValidationResult(
                        "Coach can not  have ongoing experience if he has later completed experiences!",
                        new[] { nameof(CoachExperiences) });
                }
                if (TeamId.HasValue && !(sortedExperiences.LastOrDefault().TeamId == TeamId)) {
                    yield return new ValidationResult(
                        "Coach can not be assigned to team without last experience in this team!",
                        new[] { nameof(CoachExperiences) });
                }


                for (int i = 0; i < sortedExperiences.Count; i++) {
                    var currentExperience = sortedExperiences[i];
                    DateTime? currentEndDate = !string.IsNullOrEmpty(currentExperience.EndDate) ? (DateTime?)DateTime.Parse(currentExperience.EndDate) : null;


                    foreach (var otherExperience in sortedExperiences.Where((_, index) => index != i)) {
                        DateTime otherStartDate = DateTime.Parse(otherExperience.StartDate);
                        DateTime? otherEndDate = !string.IsNullOrEmpty(otherExperience.EndDate) ? (DateTime?)DateTime.Parse(otherExperience.EndDate) : null;

                        bool isOverlap = false;
                        if (currentEndDate == null) {
                            if (currentEndDate == null || (otherEndDate.HasValue && otherStartDate < currentEndDate)) {

                                isOverlap = true;
                            }
                        } else if (otherEndDate == null) {
                            if (currentEndDate.Value >= otherStartDate) {
                                isOverlap = true;
                            }
                        } else {
                            if (otherStartDate <= currentEndDate && otherEndDate >= DateTime.Parse(currentExperience.StartDate)) {
                                isOverlap = true;
                            }
                        }

                        if (isOverlap) {
                            yield return new ValidationResult(
                                $"Experience periods must not overlap. Conflict between experience starting on {currentExperience.StartDate} and experience starting on {otherExperience.StartDate}.",
                                new[] { nameof(CoachExperiences) });
                        }
                    }
                }
            }
        }

    }
}
