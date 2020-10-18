using System;
using System.Linq;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;

namespace CanaryDeliveries.Domain.PurchaseApplication.Create
{
    public sealed class CreatePurchaseApplication
    {
        private readonly PurchaseApplicationRepository purchaseApplicationRepository;

        public CreatePurchaseApplication(PurchaseApplicationRepository purchaseApplicationRepository)
        {
            this.purchaseApplicationRepository = purchaseApplicationRepository;
        }

        public CanaryDeliveries.Domain.PurchaseApplication.PurchaseApplication Create(PurchaseApplicationCreationRequest purchaseApplicationCreationRequest)
        {
            var purchaseApplication = new CanaryDeliveries.Domain.PurchaseApplication.PurchaseApplication(
                id: Id.Create(),
                products: purchaseApplicationCreationRequest.Products.Map((product => new Product(
                    link: product.Link,
                    units: product.Units,
                    additionalInformation: product.AdditionalInformation,
                    promotionCode: product.PromotionCode))).ToList().AsReadOnly(),
                client: new Client(
                    name: purchaseApplicationCreationRequest.ClientProp.Name,
                    phoneNumber: purchaseApplicationCreationRequest.ClientProp.PhoneNumber,
                    email: purchaseApplicationCreationRequest.ClientProp.Email),
                additionalInformation: purchaseApplicationCreationRequest.AdditionalInformation,
                creationDateTime: DateTime.UtcNow);
            purchaseApplicationRepository.Create(purchaseApplication);
            return purchaseApplication;
        }
    }
}