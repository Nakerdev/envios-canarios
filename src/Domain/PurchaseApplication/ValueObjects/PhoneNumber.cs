using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class PhoneNumber : Record<PhoneNumber>
    {
        private string Value;
        
        public static Either<PhoneNumberValidationError, PhoneNumber> Create(Option<string> value)
        {
            return value
                .Map(v => new PhoneNumber(v))
                .ToEither(() => PhoneNumberValidationError.Required);
        }

        private PhoneNumber(string value)
        {
            this.Value = value;
        }
    }

    public enum PhoneNumberValidationError
    {
        Required
    }
}