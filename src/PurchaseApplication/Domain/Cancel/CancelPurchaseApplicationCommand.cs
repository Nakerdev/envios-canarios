using System;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using LanguageExt;

namespace CanaryDeliveries.PurchaseApplication.Domain.Cancel
{
    public sealed class CancelPurchaseApplicationCommand
    {
        public Id PurchaseApplicationId { get; }
        public RejectionReason RejectionReason { get; }
        
        public static Validation<
            ValidationError<GenericValidationErrorCode>, 
            CancelPurchaseApplicationCommand> Create(Dto dto)
        {
            var purchaseApplicationId = Id.Create(dto.PurchaseApplicationId);
            var rejectionReason = RejectionReason.Create(dto.RejectionReason);
            
            if (rejectionReason.IsFail || purchaseApplicationId.IsFail)
            {
                var validationErrors = Prelude.Seq<ValidationError<GenericValidationErrorCode>>();
                purchaseApplicationId.IfFail(errors => validationErrors = validationErrors.Concat(errors));
                rejectionReason.IfFail(errors => validationErrors = validationErrors.Concat(errors));
                return validationErrors;
            }
            
            return new CancelPurchaseApplicationCommand(
                purchaseApplicationId: purchaseApplicationId.IfFail(() => throw new InvalidOperationException()),
                rejectionReason: rejectionReason.IfFail(() => throw new InvalidOperationException())); 
        }

        private CancelPurchaseApplicationCommand(
            Id purchaseApplicationId, 
            RejectionReason rejectionReason)
        {
            PurchaseApplicationId = purchaseApplicationId;
            RejectionReason = rejectionReason;
        }

        public sealed class Dto 
        {
            public Option<string> PurchaseApplicationId { get; }
            public Option<string> RejectionReason { get; }

            public Dto(Option<string> purchaseApplicationId, Option<string> rejectionReason)
            {
                PurchaseApplicationId = purchaseApplicationId;
                RejectionReason = rejectionReason;
            }
        }
    }
}