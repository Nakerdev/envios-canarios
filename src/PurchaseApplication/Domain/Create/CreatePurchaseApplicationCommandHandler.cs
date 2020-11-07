using CanaryDeliveries.PurchaseApplication.Domain.Services;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;

namespace CanaryDeliveries.PurchaseApplication.Domain.Create
{
    public sealed class CreatePurchaseApplicationCommandHandler
    {
        private readonly PurchaseApplicationRepository purchaseApplicationRepository;
        private readonly TimeService timeService;

        public CreatePurchaseApplicationCommandHandler(
            PurchaseApplicationRepository purchaseApplicationRepository,
            TimeService timeService)
        {
            this.purchaseApplicationRepository = purchaseApplicationRepository;
            this.timeService = timeService;
        }

        public PurchaseApplication Create(CreatePurchaseApplicationCommand command)
        {
            var purchaseApplication = BuildPurchaseApplication(command);
            purchaseApplicationRepository.Create(purchaseApplication);
            return purchaseApplication;
        }

        private PurchaseApplication BuildPurchaseApplication(CreatePurchaseApplicationCommand command)
        {
            return new PurchaseApplication(
                id: Id.Create(),
                products: command.Products,
                client: command.ClientProp,
                additionalInformation: command.AdditionalInformation,
                creationDateTime: timeService.UtcNow());
        }
    }
}