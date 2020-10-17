namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Name
    {
        public string Value { get; }

        public Name(string value)
        {
            Value = value;
        }
    }
}