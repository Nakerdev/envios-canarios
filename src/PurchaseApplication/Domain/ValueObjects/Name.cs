using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class Name : Record<Name>
    {
        public PersistenceState State => new PersistenceState(value);
        
        private readonly string value;
        
        public static Validation<ValidationError<GenericValidationErrorCode>, Name> Create(Option<string> value)
        {
            return
                from name in ValidateRequire()
                from _1 in ValidateLenght(name)
                select BuildName(name);

            Validation<ValidationError<GenericValidationErrorCode>, string> ValidateRequire()
            {
                return value
                    .ToValidation(CreateValidationError(GenericValidationErrorCode.Required));
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateLenght(string name)
            {
                const int maxAllowedLenght = 255;
                if (name.Length > maxAllowedLenght)
                {
                    return CreateValidationError(GenericValidationErrorCode.WrongLength);
                }
                return unit;
            }

            static Name BuildName(string name)
            {
                return new Name(name); 
            }

            ValidationError<GenericValidationErrorCode> CreateValidationError(
                GenericValidationErrorCode errorCode)
            {
                return new ValidationError<GenericValidationErrorCode>(
                    fieldId: nameof(Name), 
                    errorCode: errorCode);
            }
        }

        public Name(PersistenceState persistenceState)
        {
            value = persistenceState.Value;
        }

        private Name(string value)
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