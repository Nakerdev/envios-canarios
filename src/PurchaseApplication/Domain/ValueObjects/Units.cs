using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class Units : Record<Units>
    {
        public PersistenceState State => new PersistenceState(value);
        
        private readonly int value;

        public static Validation<ValidationError<GenericValidationErrorCode>, Units> Create(Option<string> value)
        {
            return
                from units in ValidateRequire(value)
                from _1 in ValidateFormat(units)
                from _2 in ValidateValue(units)
                select BuildUnits(units);

            Validation<ValidationError<GenericValidationErrorCode>, string> ValidateRequire(Option<string> units)
            {
                return units.Match(
                    None: () => Fail<ValidationError<GenericValidationErrorCode>, string>(
                        CreateValidationError(GenericValidationErrorCode.Required)),
                    Some: Success<ValidationError<GenericValidationErrorCode>, string>);
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateFormat(string units)
            {
                if (!int.TryParse(units, out _))
                {
                    return CreateValidationError(GenericValidationErrorCode.InvalidFormat);
                };
                return unit;
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateValue(string units)
            {
                if (int.Parse(units) <= 0)
                {
                    return CreateValidationError(GenericValidationErrorCode.InvalidValue);
                };
                return unit;
            }

            static Units BuildUnits(string units)
            {
                return new Units(int.Parse(units));
            }

            ValidationError<GenericValidationErrorCode> CreateValidationError(GenericValidationErrorCode errorCode)
            {
                return new ValidationError<GenericValidationErrorCode>(fieldId: nameof(Units), errorCode: errorCode);
            }
        }
        
        public Units(PersistenceState persistenceState)
        {
            value = persistenceState.Value;
        }

        private Units(int value)
        {
            this.value = value;
        }

        public sealed class PersistenceState
        {
            public int Value { get; }

            public PersistenceState(int value)
            {
                Value = value;
            }
        }
    }
}