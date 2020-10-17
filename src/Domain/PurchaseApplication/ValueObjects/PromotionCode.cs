namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class PromotionCode
    {
        public string Value { get; }

        public PromotionCode(string value)
        {
            Value = value;
        }
    }
}