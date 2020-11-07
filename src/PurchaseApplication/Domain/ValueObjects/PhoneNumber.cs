using System.Text.RegularExpressions;
using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class PhoneNumber : Record<PhoneNumber>
    {
        private readonly string Value;
        
        public static Validation<ValidationError<GenericValidationErrorCode>, PhoneNumber> Create(Option<string> value)
        {
            return
                from phoneNumberUnparsedValue in ValidateRequire(value)
                from phoneNumber in ValidateFormat(phoneNumberUnparsedValue)
                from _ in ValidateLenght(phoneNumber)
                select phoneNumber;

            Validation<ValidationError<GenericValidationErrorCode>, string> ValidateRequire(Option<string> val)
            {
                return val.Match(
                    None: () => Fail<ValidationError<GenericValidationErrorCode>, string>(
                        CreateValidationError(GenericValidationErrorCode.Required)),
                    Some: Success<ValidationError<GenericValidationErrorCode>, string>);
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, PhoneNumber> ValidateFormat(string val)
            {
                if (Regex.Match(val, @"^(\[0-9])$").Success)
                {
                    return CreateValidationError(GenericValidationErrorCode.InvalidFormat);
                };
                return new PhoneNumber(val);
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, PhoneNumber> ValidateLenght(PhoneNumber phoneNumber)
            {
                const int maxAllowedLenght = 15;
                if (phoneNumber.Value.Length > maxAllowedLenght)
                {
                    return CreateValidationError(GenericValidationErrorCode.WrongLength);
                };
                return phoneNumber;
            }

            ValidationError<GenericValidationErrorCode> CreateValidationError(GenericValidationErrorCode errorCode)
            {
                return new ValidationError<GenericValidationErrorCode>(
                    fieldId: nameof(PhoneNumber), 
                    errorCode: errorCode);
            }
        }

        private PhoneNumber(string value)
        {
            Value = value;
        }
    }
}