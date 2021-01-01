using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class PromotionCode : Record<PromotionCode>
    {
        public PersistenceState State => new PersistenceState(Value);
        
        private readonly string Value;
        
        public static Validation<ValidationError<GenericValidationErrorCode>, PromotionCode> Create(
            Option<string> value)
        {
            return
                from promotionCode in ValidateRequire()
                from _1 in ValidateLenght(promotionCode)
                select BuildPromotionCode(promotionCode);

            Validation<ValidationError<GenericValidationErrorCode>, string> ValidateRequire()
            {
                return value
                    .ToValidation(CreateValidationError(GenericValidationErrorCode.Required));
            }
            
            Validation<ValidationError<GenericValidationErrorCode>, Unit> ValidateLenght(
                string promotionCode)
            {
                const int maxAllowedLenght = 50;
                if (promotionCode.Length > maxAllowedLenght)
                {
                    return CreateValidationError(GenericValidationErrorCode.WrongLength);
                }
                return unit;
            }

            static PromotionCode BuildPromotionCode(string promotionCode)
            {
                return new PromotionCode(promotionCode);
            }

            ValidationError<GenericValidationErrorCode> CreateValidationError(
                GenericValidationErrorCode errorCode)
            {
                return new ValidationError<GenericValidationErrorCode>(
                    fieldId: nameof(PromotionCode), 
                    errorCode: errorCode);
            }
        }

        public PromotionCode(PersistenceState persistenceState)
        {
            Value = persistenceState.Value;
        }

        private PromotionCode(string value)
        {
            Value = value;
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