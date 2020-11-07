using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Email : Record<Email>
    {
        private readonly string Value;
        
        public static Validation<ValidationError<GenericValidationErrorCode>, Email> Create(Option<string> value)
        {
            return
                from email in ValidateRequire(value)
                from _1 in ValidateFormat(email)
                from _2 in ValidateLenght(email)
                select email;

            Validation<ValidationError<GenericValidationErrorCode>, Email> ValidateRequire(
                Option<string> val)
            {
                return val
                    .Map(v => new Email(v))
                    .ToValidation(CreateValidationError(GenericValidationErrorCode.Required));
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Email> ValidateFormat(Email email)
             {
                 try
                 {
                     var mailAddress = new System.Net.Mail.MailAddress(email.Value);
                     return email;
                 }
                 catch {
                     return CreateValidationError(GenericValidationErrorCode.InvalidFormat);
                 }
             }
            
            Validation<ValidationError<GenericValidationErrorCode>, Email> ValidateLenght(Email email)
            {
                const int maxAllowedLenght = 255;
                if (email.Value.Length > maxAllowedLenght)
                {
                    return CreateValidationError(GenericValidationErrorCode.WrongLength);
                }
                return email;
            }

            ValidationError<GenericValidationErrorCode> CreateValidationError(
                GenericValidationErrorCode errorCode)
            {
                return new ValidationError<GenericValidationErrorCode>(
                    fieldId: nameof(Email), 
                    errorCode: errorCode);
            }
        }

        private Email(string value)
        {
            Value = value;
        }
    }
}