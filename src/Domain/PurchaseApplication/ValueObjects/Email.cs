using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Email : Record<Email>
    {
        private string value;
        
        public static Either<EmailValidationError, Email> Create(Option<string> value)
        {
            return value
                .Map(v => new Email(v))
                .ToEither(() => EmailValidationError.Required);
        }

        private Email(string value)
        {
            this.value = value;
        }
    }

    public enum EmailValidationError
    {
        Required
    }
}