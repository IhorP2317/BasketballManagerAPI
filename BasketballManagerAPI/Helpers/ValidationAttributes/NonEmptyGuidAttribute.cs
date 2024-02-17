using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Helpers.ValidationAttributes {
    public class NonEmptyGuidAttribute:ValidationAttribute {
        public NonEmptyGuidAttribute() : base("The {0} field must be a non-empty GUID.") {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value == null || (Guid)value == Guid.Empty) {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}
