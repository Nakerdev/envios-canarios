using CanaryDeliveries.Domain.PurchaseApplication.Create;
using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class PromotionCode : Record<PromotionCode>
    {
        private readonly string Value;
        
        public static Validation<ValidationError<PromotionCodeValidationErrorCode>, PromotionCode> Create(
            Option<string> value)
        {
            return
                from promotionCode in ValidateRequire(value)
                from _1 in ValidateLenght(promotionCode)
                select promotionCode;

            Validation<ValidationError<PromotionCodeValidationErrorCode>, PromotionCode> ValidateRequire(
                Option<string> val)
            {
                return val
                    .Map(v => new PromotionCode(v))
                    .ToValidation(CreateValidationError(PromotionCodeValidationErrorCode.Required));
            }
            
            Validation<ValidationError<PromotionCodeValidationErrorCode>, PromotionCode> ValidateLenght(
                PromotionCode promotionCode)
            {
                const int maxAllowedLenght = 50;
                if (promotionCode.Value.Length > maxAllowedLenght)
                {
                    return CreateValidationError(PromotionCodeValidationErrorCode.WrongLength);
                }
                return promotionCode;
            }

            ValidationError<PromotionCodeValidationErrorCode> CreateValidationError(
                PromotionCodeValidationErrorCode errorCode)
            {
                return new ValidationError<PromotionCodeValidationErrorCode>(
                    fieldId: nameof(PromotionCode), 
                    errorCode: errorCode);
            }
        }

        private PromotionCode(string value)
        {
            Value = value;
        }
    }

    public enum PromotionCodeValidationErrorCode
    {
        Required,
        WrongLength
    }
}