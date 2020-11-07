using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Name : Record<Name>
    {
        private readonly string Value;
        
        public static Validation<ValidationError<GenericValidationErrorCode>, Name> Create(Option<string> value)
        {
            return
                from name in ValidateRequire(value)
                from _1 in ValidateLenght(name)
                select name;

            Validation<ValidationError<GenericValidationErrorCode>, Name> ValidateRequire(
                Option<string> val)
            {
                return val
                    .Map(v => new Name(v))
                    .ToValidation(CreateValidationError(GenericValidationErrorCode.Required));
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Name> ValidateLenght(
                Name name)
            {
                const int maxAllowedLenght = 255;
                if (name.Value.Length > maxAllowedLenght)
                {
                    return CreateValidationError(GenericValidationErrorCode.WrongLength);
                }
                return name;
            }

            ValidationError<GenericValidationErrorCode> CreateValidationError(
                GenericValidationErrorCode errorCode)
            {
                return new ValidationError<GenericValidationErrorCode>(
                    fieldId: nameof(Name), 
                    errorCode: errorCode);
            }
        }

        private Name(string value)
        {
            Value = value;
        }
    }
}