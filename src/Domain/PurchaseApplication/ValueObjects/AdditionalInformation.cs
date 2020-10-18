using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class AdditionalInformation : Record<AdditionalInformation>
    {
        private string value;

        public AdditionalInformation(string value)
        {
            this.value = value;
        }
    }
}