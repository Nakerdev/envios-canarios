namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Email
    {
        public string Value { get; }

        public Email(string value)
        {
            Value = value;
        }
    }
}