using CanaryDeliveries.Domain.PurchaseApplication.Create;
using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Units : Record<Units>
    {
        private int value;

        public static Validation<ValidationError<UnitsValidationErrorCode>, Units> Create(Option<string> value)
        {
            return
                from unitsUnparsedValue in ValidateRequire(value)
                from units in ValidateFormat(unitsUnparsedValue)
                from _ in ValidateValue(units)
                select units;

            Validation<ValidationError<UnitsValidationErrorCode>, string> ValidateRequire(Option<string> val)
            {
                return val.Match(
                    None: () => Fail<ValidationError<UnitsValidationErrorCode>, string>(
                        CreateValidationError(UnitsValidationErrorCode.Required)),
                    Some: Success<ValidationError<UnitsValidationErrorCode>, string>);
            }
            
            Validation<ValidationError<UnitsValidationErrorCode>, Units> ValidateFormat(string val)
            {
                if (!int.TryParse(val, out var parsedValue))
                {
                    return CreateValidationError(UnitsValidationErrorCode.InvalidFormat);
                };
                return new Units(parsedValue);
            }
            
            Validation<ValidationError<UnitsValidationErrorCode>, Units> ValidateValue(Units units)
            {
                if (units.value <= 0)
                {
                    return CreateValidationError(UnitsValidationErrorCode.InvalidValue);
                };
                return units;
            }

            ValidationError<UnitsValidationErrorCode > CreateValidationError(UnitsValidationErrorCode errorCode)
            {
                return new ValidationError<UnitsValidationErrorCode>(fieldId: nameof(Units), errorCode: errorCode);
            }
        }

        private Units(int value)
        {
            this.value = value;
        }
    }

    public enum UnitsValidationErrorCode
    {
        Required,
        InvalidFormat,
        InvalidValue
    }
}