using System;
using System.Collections.Generic;
using CanaryDeliveries.PurchaseApplication.Domain.Entities;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;

namespace CanaryDeliveries.Tests.PurchaseApplication.Unit.Builders
{
    public static class PurchaseApplicationBuilder
    {
        public static CanaryDeliveries.PurchaseApplication.Domain.PurchaseApplication Build()
        {
            var state = new CanaryDeliveries.PurchaseApplication.Domain.PurchaseApplication.PersistenceState(
                id: new Id.PersistenceState("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                products: new List<Product.PersistenceState>
                {
                    new Product.PersistenceState(
                        link: new Link.PersistenceState("https://addidas.com/products/1"),
                        units: new Units.PersistenceState(1),
                        additionalInformation: new AdditionalInformation.PersistenceState("size: 30, color: red"),
                        promotionCode: new PromotionCode.PersistenceState("ADDIDAS-123"))
                },
                client: new Client.PersistenceState(
                    name: new Name.PersistenceState("Alfredo"),
                    phoneNumber: new PhoneNumber.PersistenceState("610232323"),
                    email: new Email.PersistenceState("alfredo@email.com")),
                additionalInformation: new AdditionalInformation.PersistenceState("Purchase additional information"),
                creationDate: new DateTime(2020, 10, 10, 12, 30, 00));
            return new CanaryDeliveries.PurchaseApplication.Domain.PurchaseApplication(state);
        }
    }
}