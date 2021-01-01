using System.Text.RegularExpressions;
using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class PhoneNumber : Record<PhoneNumber>
    {
        public PersistenceState State => new PersistenceState(value);
        
        private readonly string value;
        
        public static Validation<ValidationError<GenericValidationErrorCode>, PhoneNumber> Create(Option<string> value)
        {
            return
                from phoneNumber in ValidateRequire()
                from _1 in ValidateFormat(phoneNumber)
                from _2 in ValidateLenght(phoneNumber)
                select BuildPhoneNumber(phoneNumber);

            Validation<ValidationError<GenericValidationErrorCode>, string> ValidateRequire()
            {
                return value
                    .ToValidation(CreateValidationError(GenericValidationErrorCode.Required));
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateFormat(string phoneNumber)
            {
                if (!Regex.Match(phoneNumber, @"^[0-9]*$").Success)
                {
                    return CreateValidationError(GenericValidationErrorCode.InvalidFormat);
                };
                return unit;
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateLenght(string phoneNumber)
            {
                const int maxAllowedLenght = 15;
                if (phoneNumber.Length > maxAllowedLenght)
                {
                    return CreateValidationError(GenericValidationErrorCode.WrongLength); 
                };
                return unit;
            }

            static PhoneNumber BuildPhoneNumber(string phoneNumber)
            {
                return new PhoneNumber(phoneNumber);
            }

            ValidationError<GenericValidationErrorCode> CreateValidationError(GenericValidationErrorCode errorCode)
            {
                return new ValidationError<GenericValidationErrorCode>(
                    fieldId: nameof(PhoneNumber), 
                    errorCode: errorCode);
            }
        }

        public PhoneNumber(PersistenceState persistenceState)
        {
            value = persistenceState.Value;
        }

        private PhoneNumber(string value)
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