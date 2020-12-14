using CanaryDeliveries.PurchaseApplication.Domain.Services;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.PurchaseApplication.Domain.Cancel
{
    public sealed class CancelPurchaseApplicationCommandHandler
    {
        private readonly PurchaseApplicationRepository purchaseApplicationRepository;
        private readonly TimeService timeService;

        public CancelPurchaseApplicationCommandHandler(
            PurchaseApplicationRepository purchaseApplicationRepository,
            TimeService timeService)
        {
            this.purchaseApplicationRepository = purchaseApplicationRepository;
            this.timeService = timeService;
        }

        public Either<Error, PurchaseApplication> Cancel(CancelPurchaseApplicationCommand command)
        {
            return
                from purchaseApplication in SearchPurchaseApplicationBy(command.Id)
                from rejectedPurchaseApplication in Reject(
                    purchaseApplication: purchaseApplication, 
                    rejectionReason: command.RejectionReason)
                from _ in Update(rejectedPurchaseApplication)
                select rejectedPurchaseApplication;
        }

        private Either<Error, PurchaseApplication> SearchPurchaseApplicationBy(Id id)
        {
            return purchaseApplicationRepository
                .SearchBy(id)
                .ToEither(() => Error.PurchaseApplicationNotFound);
        }
        
        private Either<Error, PurchaseApplication> Reject(
            PurchaseApplication purchaseApplication,
            string rejectionReason)
        {
            return purchaseApplication
                .Reject(timeService.UtcNow(), rejectionReason)
                .MapLeft(_ => Error.PurchaseApplicationIsAlreadyRejected);
        }
        
        private Either<Error, Unit> Update(PurchaseApplication purchaseApplication)
        {
            purchaseApplicationRepository.Update(purchaseApplication);
            return unit;
        }
    }

    public enum Error
    {
        PurchaseApplicationIsAlreadyRejected,
        PurchaseApplicationNotFound
    }
}