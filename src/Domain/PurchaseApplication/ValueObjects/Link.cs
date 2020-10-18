using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Link : Record<Link>
    {
        private string value;

        public Link(string value)
        {
            this.value = value;
        }
    }
}