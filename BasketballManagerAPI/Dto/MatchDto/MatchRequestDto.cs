using BasketballManagerAPI.Helpers.ValidationAttributes;
using BasketballManagerAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Dto.MatchDto {
    public class MatchRequestDto : BaseEntityRequestDto, IValidatableObject {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Location is required!")]
        public string Location { get; set; } = null!;

        [Required(ErrorMessage = "Start Time is required.")]
        [DateTimeFormat("yyyy-MM-dd HH:mm:ss", ErrorMessage = "DateTime must be in the format yyyy-MM-dd HH:mm:ss.")]
        public string StartTime { get; set; } = null!;
        [Required(ErrorMessage = "End Time is required.")]
        [DateTimeFormat("yyyy-MM-dd HH:mm:ss", ErrorMessage = "DateTime must be in the format yyyy-MM-dd HH:mm:ss.")]
        public string? EndTime { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Match Status is required!")]
        [EnumValue(typeof(MatchStatus))]
        public string Status { get; set; } = null!;
        [NonEmptyGuid(ErrorMessage = "Home Team ID must be a non-empty GUID.")]
        public Guid HomeTeamId { get; set; }
        [NonEmptyGuid(ErrorMessage = "Away Team ID must be a non-empty GUID.")]
        public Guid AwayTeamId { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (HomeTeamId == AwayTeamId)
                yield return new ValidationResult("One team can only participate in match once!",
                    new[] { nameof(HomeTeamId) });
            if (StartTime != null && EndTime != null) {
                
                if (Enum.TryParse<MatchStatus>(Status, out var matchStatus) && matchStatus != MatchStatus.Completed)
                {
                    yield return new ValidationResult(
                        "Not Competed Match can not have End Time.",
                        new[] { nameof(Status) });
                }
                if (DateTime.TryParse(StartTime, out var startTime) && DateTime.TryParse(EndTime, out var endTime) && endTime <= startTime)
                {
                    yield return new ValidationResult(
                        "End Time must be after Start Time.",
                        new[] { nameof(EndTime) });
                }
            }
        }

    }
}
