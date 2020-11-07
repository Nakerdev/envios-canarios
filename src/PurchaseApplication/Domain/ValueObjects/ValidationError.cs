namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class ValidationError<T>
    {
        public string FieldId { get; }
        public T ErrorCode { get; }

        public ValidationError(string fieldId, T errorCode)
        {
            FieldId = fieldId;
            ErrorCode = errorCode;
        }
    }

    public enum GenericValidationErrorCode
    {
        Required,
        InvalidValue,
        InvalidFormat,
        WrongLength
    }
}