using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Name : Record<Name>
    {
        private string value;
        
        public static Either<NameValidationError, Name> Create(Option<string> value)
        {
            return value
                .Map(v => new Name(v))
                .ToEither(() => NameValidationError.Required);
        }

        private Name(string value)
        {
            this.value = value;
        }
    }

    public enum NameValidationError
    {
        Required
    }
}