using System.Linq;
using CanaryDeliveries.Domain.PurchaseApplication.Entities;
using CanaryDeliveries.Domain.PurchaseApplication.Services;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;

namespace CanaryDeliveries.Domain.PurchaseApplication.Create
{
    public sealed class CreatePurchaseApplication
    {
        private readonly PurchaseApplicationRepository purchaseApplicationRepository;
        private readonly TimeService timeService;

        public CreatePurchaseApplication(
            PurchaseApplicationRepository purchaseApplicationRepository,
            TimeService timeService)
        {
            this.purchaseApplicationRepository = purchaseApplicationRepository;
            this.timeService = timeService;
        }

        public PurchaseApplication Create(PurchaseApplicationCreationRequest purchaseApplicationCreationRequest)
        {
            var purchaseApplication = BuildPurchaseApplication(purchaseApplicationCreationRequest);
            purchaseApplicationRepository.Create(purchaseApplication);
            return purchaseApplication;
        }

        private PurchaseApplication BuildPurchaseApplication(PurchaseApplicationCreationRequest purchaseApplicationCreationRequest)
        {
            return new PurchaseApplication(
                id: Id.Create(),
                products: purchaseApplicationCreationRequest.Products.Map((BuildProduct)).ToList().AsReadOnly(),
                client: purchaseApplicationCreationRequest.ClientProp,
                additionalInformation: purchaseApplicationCreationRequest.AdditionalInformation,
                creationDateTime: timeService.UtcNow());

            Product BuildProduct(Product product)
            {
                return new Product(
                    link: product.Link,
                    units: product.Units,
                    additionalInformation: product.AdditionalInformation,
                    promotionCode: product.PromotionCode);
            }
        }
    }
}