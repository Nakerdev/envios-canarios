using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Email : Record<Email>
    {
        private string value;

        public Email(string value)
        {
            this.value = value;
        }
    }
}