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

        public PurchaseApplication Create(PurchaseApplicationCreationRequest purchaseApplicationCreationRequest)
        {
            var purchaseApplication = BuildPurchaseApplication(purchaseApplicationCreationRequest);
            purchaseApplicationRepository.Create(purchaseApplication);
            return purchaseApplication;
        }

        private static PurchaseApplication BuildPurchaseApplication(PurchaseApplicationCreationRequest purchaseApplicationCreationRequest)
        {
            return new PurchaseApplication(
                id: Id.Create(),
                products: purchaseApplicationCreationRequest.Products.Map((BuildProduct)).ToList().AsReadOnly(),
                client: BuildClient(),
                additionalInformation: purchaseApplicationCreationRequest.AdditionalInformation,
                creationDateTime: DateTime.UtcNow);

            Product BuildProduct(PurchaseApplicationCreationRequest.Product product)
            {
                return new Product(
                    link: product.Link,
                    units: product.Units,
                    additionalInformation: product.AdditionalInformation,
                    promotionCode: product.PromotionCode);
            }

            Client BuildClient()
            {
                return new Client(
                    name: purchaseApplicationCreationRequest.ClientProp.Name,
                    phoneNumber: purchaseApplicationCreationRequest.ClientProp.PhoneNumber,
                    email: purchaseApplicationCreationRequest.ClientProp.Email);
            }
        }
    }
}