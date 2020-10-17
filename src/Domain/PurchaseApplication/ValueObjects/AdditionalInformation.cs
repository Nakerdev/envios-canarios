namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class AdditionalInformation
    {
        public string Value { get; }

        public AdditionalInformation(string value)
        {
            Value = value;
        }
    }
}