using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BasketballManagerAPI.Helpers.ValidationAttributes
{
    public class DateTimeFormatAttribute : ValidationAttribute
    {
        private readonly string _dateFormat;

        public DateTimeFormatAttribute(string dateFormat)
        {
            _dateFormat = dateFormat;
            ErrorMessage = $"The date must be in the format {dateFormat}.";
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            if (value is string dateString)
            {
                if (DateTime.TryParseExact(dateString, _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return new ValidationResult("Invalid date format.");
        }

    }
}
