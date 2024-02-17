using System.ComponentModel.DataAnnotations;
using BasketballManagerAPI.Helpers.ValidationAttributes;

namespace BasketballManagerAPI.Dto.MatchDto {
    public class MatchFiltersDto: IValidatableObject {
        private const int BasketballFoundationDate = 1892;
        private const int FirstMonth = 1;
        private const int LastMonth = 12;

        public string? Year { get; set; }
        public string? Month { get; set; }
        public string? TeamName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than 0")]
        public int PageSize { get; set; }
       

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            

            if (Year != null && int.TryParse(Year, out var year)) {
                
                if (year < BasketballFoundationDate || year > DateTime.Now.Year + 1) {
                    yield return new ValidationResult(
                        "Year must be a valid four-digit number and cannot be further then 1 year from .",
                        new[] { nameof(Year) });
                }
            } else if (Year != null) 
            {
                yield return new ValidationResult(
                    "Year must be a valid four-digit year.",
                    new[] { nameof(Year) });
            }
            
            if (Month != null && int.TryParse(Month, out var month)) {
                if (month < FirstMonth || month > LastMonth) {
                    yield return new ValidationResult(
                        "Month must be a number between 1 and 12.",
                        new[] { nameof(Month) });
                }
            } else if (Month != null) 
            {
                yield return new ValidationResult(
                    "Month must be a number between 1 and 12.",
                    new[] { nameof(Month) });
            }

        }

    }
}
