using CanaryDeliveries.Domain.PurchaseApplication.Create;
using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Email : Record<Email>
    {
        private readonly string Value;
        
        public static Validation<ValidationError<EmailValidationErrorCode>, Email> Create(Option<string> value)
        {
            return
                from email in ValidateRequire(value)
                from _1 in ValidateFormat(email)
                from _2 in ValidateLenght(email)
                select email;

            Validation<ValidationError<EmailValidationErrorCode>, Email> ValidateRequire(
                Option<string> val)
            {
                return val
                    .Map(v => new Email(v))
                    .ToValidation(CreateValidationError(EmailValidationErrorCode.Required));
            }
            
            Validation<ValidationError<EmailValidationErrorCode>, Email> ValidateFormat(Email email)
             {
                 try
                 {
                     var mailAddress = new System.Net.Mail.MailAddress(email.Value);
                     return email;
                 }
                 catch {
                     return CreateValidationError(EmailValidationErrorCode.InvalidFormat);
                 }
             }
            
            Validation<ValidationError<EmailValidationErrorCode>, Email> ValidateLenght(Email email)
            {
                const int maxAllowedLenght = 255;
                if (email.Value.Length > maxAllowedLenght)
                {
                    return CreateValidationError(EmailValidationErrorCode.WrongLength);
                }
                return email;
            }

            ValidationError<EmailValidationErrorCode> CreateValidationError(
                EmailValidationErrorCode errorCode)
            {
                return new ValidationError<EmailValidationErrorCode>(
                    fieldId: nameof(Email), 
                    errorCode: errorCode);
            }
        }

        private Email(string value)
        {
            Value = value;
        }
    }

    public enum EmailValidationErrorCode
    {
        Required,
        WrongLength,
        InvalidFormat
    }
}