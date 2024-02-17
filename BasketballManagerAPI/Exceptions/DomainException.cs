namespace BasketballManagerAPI.Exceptions {
    public class DomainException: Exception
    {
        public string FieldName { get; } = null!;
        public Object FieldValue { get; } = null!;
        public DomainException(string fieldName, object fieldValue)
            : base($"The field '{fieldName}' with value '{fieldValue}' violates a uniqueness constraint.") {
            FieldName = fieldName;
            FieldValue = fieldValue;
        }
    }
}
