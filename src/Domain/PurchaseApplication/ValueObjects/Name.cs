using CanaryDeliveries.Domain.PurchaseApplication.Create;
using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Name : Record<Name>
    {
        private string value;
        
        public static Validation<ValidationError<NameValidationErrorCode>, Name> Create(
            Option<string> value)
        {
            return
                from name in ValidateRequire(value)
                from _1 in ValidateLenght(name)
                select name;

            Validation<ValidationError<NameValidationErrorCode>, Name> ValidateRequire(
                Option<string> val)
            {
                return val
                    .Map(v => new Name(v))
                    .ToValidation(CreateValidationError(NameValidationErrorCode.Required));
            }
            
            Validation<ValidationError<NameValidationErrorCode>, Name> ValidateLenght(
                Name name)
            {
                const int maxAllowedLenght = 255;
                if (name.value.Length > maxAllowedLenght)
                {
                    return CreateValidationError(NameValidationErrorCode.WrongLength);
                }
                return name;
            }

            ValidationError<NameValidationErrorCode> CreateValidationError(
                NameValidationErrorCode errorCode)
            {
                return new ValidationError<NameValidationErrorCode>(
                    fieldId: nameof(Name), 
                    errorCode: errorCode);
            }
        }

        private Name(string value)
        {
            this.value = value;
        }
    }

    public enum NameValidationErrorCode
    {
        Required,
        WrongLength
    }
}