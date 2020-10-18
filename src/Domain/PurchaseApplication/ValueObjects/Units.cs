using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Units : Record<Units>
    {
        private int value;

        public Units(int value)
        {
            this.value = value;
        }
    }
}