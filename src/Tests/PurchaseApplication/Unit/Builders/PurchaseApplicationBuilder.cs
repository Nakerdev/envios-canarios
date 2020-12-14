using System;
using System.Collections.Generic;
using CanaryDeliveries.PurchaseApplication.Domain.Entities;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;

namespace CanaryDeliveries.Tests.PurchaseApplication.Unit.Builders
{
    public static class PurchaseApplicationBuilder
    {
        public static CanaryDeliveries.PurchaseApplication.Domain.PurchaseApplication Build(
            string id = "3F2504E0-4F89-11D3-9A0C-0305E82C3301")
        {
            var state = new CanaryDeliveries.PurchaseApplication.Domain.PurchaseApplication.PersistenceStateDto(
                id: new Id.PersistenceState(id),
                products: new List<Product.PersistenceState>
                {
                    new Product.PersistenceState(
                        id: new Id.PersistenceState("F07FC790-05BA-4E15-A935-98185102192A"), 
                        link: new Link.PersistenceState("https://addidas.com/products/1"),
                        units: new Units.PersistenceState(1),
                        additionalInformation: new AdditionalInformation.PersistenceState("size: 30, color: red"),
                        promotionCode: new PromotionCode.PersistenceState("ADDIDAS-123"))
                },
                client: new Client.PersistenceState(
                    id: new Id.PersistenceState("5A687AE7-A864-46FC-A4B6-8411FEF212B6"), 
                    name: new Name.PersistenceState("Alfredo"),
                    phoneNumber: new PhoneNumber.PersistenceState("610232323"),
                    email: new Email.PersistenceState("alfredo@email.com")),
                additionalInformation: new AdditionalInformation.PersistenceState("Purchase additional information"),
                creationDateTime: new DateTime(2020, 10, 10, 12, 30, 00));
            return new CanaryDeliveries.PurchaseApplication.Domain.PurchaseApplication(state);
        }
    }
}