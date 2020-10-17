namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Units
    {
        public int Value { get; }

        public Units(int value)
        {
            Value = value;
        }
    }
}