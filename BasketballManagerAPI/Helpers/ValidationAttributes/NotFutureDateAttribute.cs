using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BasketballManagerAPI.Helpers.ValidationAttributes {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotFutureDateAttribute : ValidationAttribute {
        public NotFutureDateAttribute() : base("The {0} cannot be in the future.") {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value is string dateString && DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)) {
                if (date > DateTime.UtcNow.Date) {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            } else {
                return new ValidationResult($"The {validationContext.DisplayName} is not a valid date.");
            }

            return ValidationResult.Success;
        }
    }
}
