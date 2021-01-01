using LanguageExt;
using static LanguageExt.Prelude;

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
                from additionalInformation in ValidateRequire()
                from _1 in ValidateLenght(additionalInformation)
                select BuildAdditionalInformation(additionalInformation);

            Validation<ValidationError<GenericValidationErrorCode>, string> ValidateRequire()
            {
                return value
                    .ToValidation(CreateValidationError(GenericValidationErrorCode.Required));
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateLenght(string additionalInformation)
            {
                const int maxAllowedLenght = 1000;
                if (additionalInformation.Length > maxAllowedLenght)
                {
                    return CreateValidationError(GenericValidationErrorCode.WrongLength);
                }
                return unit;
            }
            
            static AdditionalInformation BuildAdditionalInformation(string additionalInformation)
            {
                return new AdditionalInformation(additionalInformation);
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