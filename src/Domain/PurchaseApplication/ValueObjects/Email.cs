using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Email : Record<Email>
    {
        private readonly string Value;
        
        public static Either<EmailValidationError, Email> Create(Option<string> value)
        {
            return value
                .Map(v => new Email(v))
                .ToEither(() => EmailValidationError.Required);
        }

        private Email(string value)
        {
            Value = value;
        }
    }

    public enum EmailValidationError
    {
        Required
    }
}