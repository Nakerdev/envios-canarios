using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class Units : Record<Units>
    {
        private readonly int Value;

        public static Validation<ValidationError<GenericValidationErrorCode>, Units> Create(Option<string> value)
        {
            return
                from unitsUnparsedValue in ValidateRequire(value)
                from units in ValidateFormat(unitsUnparsedValue)
                from _ in ValidateValue(units)
                select units;

            Validation<ValidationError<GenericValidationErrorCode>, string> ValidateRequire(Option<string> val)
            {
                return val.Match(
                    None: () => Fail<ValidationError<GenericValidationErrorCode>, string>(
                        CreateValidationError(GenericValidationErrorCode.Required)),
                    Some: Success<ValidationError<GenericValidationErrorCode>, string>);
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Units> ValidateFormat(string val)
            {
                if (!int.TryParse(val, out var parsedValue))
                {
                    return CreateValidationError(GenericValidationErrorCode.InvalidFormat);
                };
                return new Units(parsedValue);
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Units> ValidateValue(Units units)
            {
                if (units.Value <= 0)
                {
                    return CreateValidationError(GenericValidationErrorCode.InvalidValue);
                };
                return units;
            }

            ValidationError<GenericValidationErrorCode> CreateValidationError(GenericValidationErrorCode errorCode)
            {
                return new ValidationError<GenericValidationErrorCode>(fieldId: nameof(Units), errorCode: errorCode);
            }
        }

        private Units(int value)
        {
            Value = value;
        }
    }
}