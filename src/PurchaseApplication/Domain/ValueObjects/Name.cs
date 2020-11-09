using LanguageExt;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class Name : Record<Name>
    {
        public PersistenceState State => new PersistenceState(value);
        
        private readonly string value;
        
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
                if (name.value.Length > maxAllowedLenght)
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