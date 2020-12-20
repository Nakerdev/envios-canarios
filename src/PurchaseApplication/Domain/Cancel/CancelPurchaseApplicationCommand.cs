using System;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using LanguageExt;

namespace CanaryDeliveries.PurchaseApplication.Domain.Cancel
{
    public sealed class CancelPurchaseApplicationCommand
    {
        public Id Id { get; }
        public RejectionReason RejectionReason { get; }
        
        public static Validation<
            ValidationError<GenericValidationErrorCode>, 
            CancelPurchaseApplicationCommand> Create(Dto dto)
        {
            var rejectionReason = RejectionReason.Create(dto.RejectionReason);
            
            if (rejectionReason.IsFail)
            {
                var validationErrors = Prelude.Seq<ValidationError<GenericValidationErrorCode>>();
                rejectionReason.IfFail(errors => validationErrors = validationErrors.Concat(errors));
                return validationErrors;
            }
            
            return new CancelPurchaseApplicationCommand(
                id: Id.Create(dto.Id),
                rejectionReason: rejectionReason.IfFail(() => throw new InvalidOperationException())); 
        }

        private CancelPurchaseApplicationCommand(Id id, RejectionReason rejectionReason)
        {
            Id = id;
            RejectionReason = rejectionReason;
        }

        public sealed class Dto 
        {
            public string Id { get; }
            public Option<string> RejectionReason { get; }

            public Dto(string id, Option<string> rejectionReason)
            {
                Id = id;
                RejectionReason = rejectionReason;
            }
        }
    }
}