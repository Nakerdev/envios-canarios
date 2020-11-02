using System.Text.RegularExpressions;
using CanaryDeliveries.Domain.PurchaseApplication.Create;
using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class PhoneNumber : Record<PhoneNumber>
    {
        private readonly string Value;
        
        public static Validation<ValidationError<PhoneNumberValidationErrorCode>, PhoneNumber> Create(Option<string> value)
        {
            return
                from phoneNumberUnparsedValue in ValidateRequire(value)
                from phoneNumber in ValidateFormat(phoneNumberUnparsedValue)
                from _ in ValidateLenght(phoneNumber)
                select phoneNumber;

            Validation<ValidationError<PhoneNumberValidationErrorCode>, string> ValidateRequire(Option<string> val)
            {
                return val.Match(
                    None: () => Fail<ValidationError<PhoneNumberValidationErrorCode>, string>(
                        CreateValidationError(PhoneNumberValidationErrorCode.Required)),
                    Some: Success<ValidationError<PhoneNumberValidationErrorCode>, string>);
            }
            
            Validation<ValidationError<PhoneNumberValidationErrorCode>, PhoneNumber> ValidateFormat(string val)
            {
                if (Regex.Match(val, @"^(\[0-9])$").Success)
                {
                    return CreateValidationError(PhoneNumberValidationErrorCode.InvalidFormat);
                };
                return new PhoneNumber(val);
            }
            
            Validation<ValidationError<PhoneNumberValidationErrorCode>, PhoneNumber> ValidateLenght(PhoneNumber phoneNumber)
            {
                const int maxAllowedLenght = 15;
                if (phoneNumber.Value.Length > maxAllowedLenght)
                {
                    return CreateValidationError(PhoneNumberValidationErrorCode.WrongLenght);
                };
                return phoneNumber;
            }

            ValidationError<PhoneNumberValidationErrorCode> CreateValidationError(PhoneNumberValidationErrorCode errorCode)
            {
                return new ValidationError<PhoneNumberValidationErrorCode>(
                    fieldId: nameof(PhoneNumber), 
                    errorCode: errorCode);
            }
        }

        private PhoneNumber(string value)
        {
            Value = value;
        }
    }

    public enum PhoneNumberValidationErrorCode
    {
        Required,
        InvalidFormat,
        WrongLenght
    }
}