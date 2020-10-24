using System.Diagnostics;
using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class PromotionCode : Record<PromotionCode>
    {
        private string value;
        
        public static Either<PromotionCodeValidationError, PromotionCode> Create(Option<string> value)
        {
            return value
                .Map(v => new PromotionCode(v))
                .ToEither(() => PromotionCodeValidationError.Required);
        }

        public PromotionCode(string value)
        {
            this.value = value;
        }
    }

    public enum PromotionCodeValidationError
    {
        Required
    }
}