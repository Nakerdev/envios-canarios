namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class PhoneNumber
    {
        public string Value { get; }

        public PhoneNumber(string value)
        {
            Value = value;
        }
    }
}