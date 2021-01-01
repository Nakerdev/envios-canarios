using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class Email : Record<Email>
    {
        public PersistenceState State => new PersistenceState(value);
        
        private readonly string value;
        
        public static Validation<ValidationError<GenericValidationErrorCode>, Email> Create(Option<string> value)
        {
            return
                from email in ValidateRequire()
                from _1 in ValidateFormat(email)
                from _2 in ValidateLenght(email)
                select BuildEmail(email);

            Validation<ValidationError<GenericValidationErrorCode>, string> ValidateRequire()
            {
                return value
                    .ToValidation(CreateValidationError(GenericValidationErrorCode.Required));
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateFormat(string email)
             {
                 try
                 {
                     var mailAddress = new System.Net.Mail.MailAddress(email);
                     return unit;
                 }
                 catch {
                     return CreateValidationError(GenericValidationErrorCode.InvalidFormat);
                 }
             }
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateLenght(string email)
            {
                const int maxAllowedLenght = 255;
                if (email.Length > maxAllowedLenght)
                {
                    return CreateValidationError(GenericValidationErrorCode.WrongLength);
                }
                return unit;
            }
            
            static Email BuildEmail(string email)
            {
                return new Email(email);
            }

            ValidationError<GenericValidationErrorCode> CreateValidationError(
                GenericValidationErrorCode errorCode)
            {
                return new ValidationError<GenericValidationErrorCode>(
                    fieldId: nameof(Email), 
                    errorCode: errorCode);
            }
        }

        public Email(PersistenceState persistenceState)
        {
            value = persistenceState.Value;
        }

        private Email(string value)
        {
            this.value = value;
        }

        public sealed class PersistenceState
        {
            public string Value { get; }

            public PersistenceState(string value)
            {
                Value = value;
            }
        }
    }
}