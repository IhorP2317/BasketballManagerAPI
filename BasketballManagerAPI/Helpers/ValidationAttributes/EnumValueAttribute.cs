using System.ComponentModel.DataAnnotations;

namespace BasketballManagerAPI.Helpers.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class EnumValueAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public EnumValueAttribute(Type enumType)
        {
            if (enumType is null || !enumType.IsEnum)
            {
                throw new ArgumentException("The provided type is not an enum.", nameof(enumType));
            }

            _enumType = enumType;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext) {
            var success = Enum.TryParse(_enumType, value?.ToString(), true, out var parsed);

            return success
                ? ValidationResult.Success
                : new ValidationResult($"{value} is not a valid {_enumType.Name} value.");
        }
    }
}
