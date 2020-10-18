using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Name : Record<Name>
    {
        private string value;

        public Name(string value)
        {
            this.value = value;
        }
    }
}