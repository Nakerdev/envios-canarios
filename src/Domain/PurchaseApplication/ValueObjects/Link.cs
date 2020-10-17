namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Link
    {
        public string Value { get; }

        public Link(string value)
        {
            Value = value;
        }
    }
}