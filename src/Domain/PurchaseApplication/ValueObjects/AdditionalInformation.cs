using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class AdditionalInformation : Record<AdditionalInformation>
    {
        private string value;
        
        public static Either<AdditionalInformationValidationError, AdditionalInformation> Create(Option<string> value)
        {
            return value
                .Map(v => new AdditionalInformation(v))
                .ToEither(() => AdditionalInformationValidationError.Required);
        }

        public AdditionalInformation(string value)
        {
            this.value = value;
        }
    }

    public enum AdditionalInformationValidationError
    {
        Required
    }
}