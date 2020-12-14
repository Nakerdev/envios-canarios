using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using LanguageExt;

namespace CanaryDeliveries.PurchaseApplication.Domain.Cancel
{
    public sealed class CancelPurchaseApplicationCommand
    {
        public Id Id { get; }
        public string RejectionReason { get; }
        
        public static Validation<
            ValidationError<GenericValidationErrorCode>, 
            CancelPurchaseApplicationCommand> Create(Dto dto)
        {
            return new CancelPurchaseApplicationCommand(
                id: Id.Create(dto.Id),
                rejectionReason: dto.Id); 
        }

        private CancelPurchaseApplicationCommand(Id id, string rejectionReason)
        {
            Id = id;
            RejectionReason = rejectionReason;
        }

        public sealed class Dto 
        {
            public string Id { get; }
            public string RejectionReason { get; }

            public Dto(string id, string rejectionReason)
            {
                Id = id;
                RejectionReason = rejectionReason;
            }
        }
    }
}