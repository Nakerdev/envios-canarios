using LanguageExt;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class AdditionalInformation : Record<AdditionalInformation>
    {
        public PersistenceState State => new PersistenceState(value);
        
        private readonly string value;
        
        public static Validation<ValidationError<GenericValidationErrorCode>, AdditionalInformation> Create(
            Option<string> value)
        {
            return
                from additionalInformation in ValidateRequire(value)
                from _1 in ValidateLenght(additionalInformation )
                select additionalInformation;

            Validation<ValidationError<GenericValidationErrorCode>, AdditionalInformation> ValidateRequire(
                Option<string> val)
            {
                return val
                    .Map(v => new AdditionalInformation(v))
                    .ToValidation(CreateValidationError(GenericValidationErrorCode.Required));
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, AdditionalInformation> ValidateLenght(
                AdditionalInformation additionalInformation)
            {
                const int maxAllowedLenght = 1000;
                if (additionalInformation.value.Length > maxAllowedLenght)
                {
                    return CreateValidationError(GenericValidationErrorCode.WrongLength);
                }
                return additionalInformation;
            }

            ValidationError<GenericValidationErrorCode> CreateValidationError(
                GenericValidationErrorCode errorCode)
            {
                return new ValidationError<GenericValidationErrorCode>(
                    fieldId: nameof(AdditionalInformation), 
                    errorCode: errorCode);
            }
        }
        
        public AdditionalInformation(PersistenceState persistenceState)
        {
            value = persistenceState.Value;
        }
        
        private AdditionalInformation(string value)
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