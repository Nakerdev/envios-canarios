using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class PhoneNumber : Record<PhoneNumber>
    {
        private string Value;

        public PhoneNumber(string value)
        {
            this.Value = value;
        }
    }
}