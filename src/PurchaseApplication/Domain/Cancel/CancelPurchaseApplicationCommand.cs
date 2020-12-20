using System;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using LanguageExt;

namespace CanaryDeliveries.PurchaseApplication.Domain.Cancel
{
    public sealed class CancelPurchaseApplicationCommand
    {
        public Id Id { get; }
        public Rejection Rejection { get; }
        
        public static Validation<
            ValidationError<GenericValidationErrorCode>, 
            CancelPurchaseApplicationCommand> Create(Dto dto)
        {
            var reject = Rejection.Create(optionalDateTime: DateTime.UtcNow.ToString(), dto.RejectionReason);
            
            if (reject.IsFail)
            {
                var validationErrors = Prelude.Seq<ValidationError<GenericValidationErrorCode>>();
                reject.IfFail(errors => validationErrors = validationErrors.Concat(errors));
                return validationErrors;
            }
            
            return new CancelPurchaseApplicationCommand(
                id: Id.Create(dto.Id),
                rejection: reject.IfFail(() => throw new InvalidOperationException())); 
        }

        private CancelPurchaseApplicationCommand(Id id, Rejection rejection)
        {
            Id = id;
            Rejection = rejection;
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