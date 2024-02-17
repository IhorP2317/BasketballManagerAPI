using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Helpers.ValidationAttributes {
    public class MinimumAgeAttribute : ValidationAttribute {
        private readonly int _minimumAge;

        public MinimumAgeAttribute(int minimumAge) {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value is string dateString && DateOnly.TryParse(dateString, out DateOnly dateOfBirth)) {
               
                var today = DateOnly.FromDateTime(DateTime.Today);
                int age = today.Year - dateOfBirth.Year;
                if (dateOfBirth > today.AddYears(-age)) age--;

                if (age < _minimumAge) {
                    return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
                }
            } else {
                return new ValidationResult($"The {validationContext.DisplayName} field is incorrectly formatted or missing.");
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage(string fieldName) {
            return $"{fieldName} indicates an age less than {_minimumAge} years.";
        }
    }
}
