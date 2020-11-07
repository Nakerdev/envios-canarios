using CanaryDeliveries.Domain.PurchaseApplication.Services;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;

namespace CanaryDeliveries.Domain.PurchaseApplication.Create
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