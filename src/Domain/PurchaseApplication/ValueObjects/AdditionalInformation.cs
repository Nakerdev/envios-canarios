using CanaryDeliveries.Domain.PurchaseApplication.Create;
using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class AdditionalInformation : Record<AdditionalInformation>
    {
        private string value;
        
        public static Validation<ValidationError<AdditionalInformationValidationErrorCode>, AdditionalInformation> Create(
            Option<string> value)
        {
            return
                from additionalInformation in ValidateRequire(value)
                from _1 in ValidateLenght(additionalInformation )
                select additionalInformation;

            Validation<ValidationError<AdditionalInformationValidationErrorCode>, AdditionalInformation> ValidateRequire(
                Option<string> val)
            {
                return val
                    .Map(v => new AdditionalInformation(v))
                    .ToValidation(CreateValidationError(AdditionalInformationValidationErrorCode.Required));
            }
            
            Validation<ValidationError<AdditionalInformationValidationErrorCode>, AdditionalInformation> ValidateLenght(
                AdditionalInformation additionalInformation)
            {
                const int maxAllowedLenght = 1000;
                if (additionalInformation.value.Length > maxAllowedLenght)
                {
                    return CreateValidationError(AdditionalInformationValidationErrorCode.WrongLength);
                }
                return additionalInformation;
            }

            ValidationError<AdditionalInformationValidationErrorCode> CreateValidationError(
                AdditionalInformationValidationErrorCode errorCode)
            {
                return new ValidationError<AdditionalInformationValidationErrorCode>(
                    fieldId: nameof(AdditionalInformation), 
                    errorCode: errorCode);
            }
        }

        public AdditionalInformation(string value)
        {
            this.value = value;
        }
    }

    public enum AdditionalInformationValidationErrorCode
    {
        Required,
        WrongLength
    }
}