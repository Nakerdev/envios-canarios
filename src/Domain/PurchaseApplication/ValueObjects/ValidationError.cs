namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
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
}