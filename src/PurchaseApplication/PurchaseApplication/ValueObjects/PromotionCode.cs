using CanaryDeliveries.Domain.PurchaseApplication.Create;
using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class PromotionCode : Record<PromotionCode>
    {
        private readonly string Value;
        
        public static Validation<ValidationError<GenericValidationErrorCode>, PromotionCode> Create(
            Option<string> value)
        {
            return
                from promotionCode in ValidateRequire(value)
                from _1 in ValidateLenght(promotionCode)
                select promotionCode;

            Validation<ValidationError<GenericValidationErrorCode>, PromotionCode> ValidateRequire(
                Option<string> val)
            {
                return val
                    .Map(v => new PromotionCode(v))
                    .ToValidation(CreateValidationError(GenericValidationErrorCode.Required));
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, PromotionCode> ValidateLenght(
                PromotionCode promotionCode)
            {
                const int maxAllowedLenght = 50;
                if (promotionCode.Value.Length > maxAllowedLenght)
                {
                    return CreateValidationError(GenericValidationErrorCode.WrongLength);
                }
                return promotionCode;
            }

            ValidationError<GenericValidationErrorCode> CreateValidationError(
                GenericValidationErrorCode errorCode)
            {
                return new ValidationError<GenericValidationErrorCode>(
                    fieldId: nameof(PromotionCode), 
                    errorCode: errorCode);
            }
        }

        private PromotionCode(string value)
        {
            Value = value;
        }
    }
}