using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Link : Record<Link>
    {
        private string value;
        
        public static Either<LinkValidationError, Link> Create(Option<string> value)
        {
            return value
                .Map(v => new Link(v))
                .ToEither(() => LinkValidationError.Required);
        }

        private Link(string value)
        {
            this.value = value;
        }
    }

    public enum LinkValidationError
    {
        Required
    }
}