using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class AdditionalInformation : Record<AdditionalInformation>
    {
        private readonly string Value;
        
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
                if (additionalInformation.Value.Length > maxAllowedLenght)
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

        private AdditionalInformation(string value)
        {
            Value = value;
        }
    }
}