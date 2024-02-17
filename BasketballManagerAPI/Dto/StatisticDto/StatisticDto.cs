using System.ComponentModel.DataAnnotations;
using BasketballManagerAPI.Helpers.ValidationAttributes;
using BasketballManagerAPI.Models;

namespace BasketballManagerAPI.Dto.StatisticDto {
    public class StatisticDto: IValidatableObject {
        private static readonly TimeSpan QuarterLength = TimeSpan.FromMinutes(12);
        private static readonly TimeSpan OverTimeLength = TimeSpan.FromMinutes(5);
        [NonEmptyGuid(ErrorMessage = "Match Id is Required!")]
        public Guid MatchId { get; set; }
        [NonEmptyGuid(ErrorMessage = "Match Id is Required!")]
        public Guid PlayerId { get; set; }
        [Required(ErrorMessage = "Time Unit is Required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Time unit must be greater than 0")]
        public int? TimeUnit { get; set; }
        public int OnePointShotHitCount { get; set; }
        public int OnePointShotMissCount { get; set; }
        public int TwoPointShotHitCount { get; set; }
        public int TwoPointShotMissCount { get; set; }
        public int ThreePointShotHitCount { get; set; }
        public int ThreePointShotMissCount { get; set; }
        public int AssistCount { get; set; }
        public int OffensiveReboundCount { get; set; }
        public int DefensiveReboundCount { get; set; }
        public int StealCount { get; set; }
        public int BlockCount { get; set; }
        public int TurnoverCount { get; set; }

        [Required(ErrorMessage = "Court time is Required!")]
        [DateTimeFormat("HH:mm:ss", ErrorMessage = "Court time must be in the format mm:ss!")]
        public string CourtTime { get; set; } = null!;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
          
            if (DateTime.TryParse(CourtTime, out var courtTimeDateTime)) {
                var courtTimeSpan = courtTimeDateTime.TimeOfDay;

               
                if (TimeUnit < 5) {
                    if (courtTimeSpan > QuarterLength)
                    {
                        yield return new ValidationResult(
                            "Player cannot have court time bigger than quarter length.",
                            new[] { nameof(CourtTime) });
                    }
                } else {
                    if (courtTimeSpan > OverTimeLength)
                        yield return new ValidationResult(
                            "Player cannot have court time bigger than overtime length.",
                            new[] { nameof(CourtTime) });
                }
            } else {
                yield return new ValidationResult(
                    "Court time is not in the correct format.",
                    new[] { nameof(CourtTime) });
            }
        }
    }
}
