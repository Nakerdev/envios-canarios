using System;
using System.Collections.Generic;
using CanaryDeliveries.Domain.PurchaseApplication.Entities;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication
{
    public sealed class PurchaseApplication
    {
        public Id Id { get; }
        public IReadOnlyList<Product> Products { get; }
        public Client Client { get; }
        public Option<AdditionalInformation> AdditionalInformation { get; }
        public DateTime CreationDateTime { get; }

        public PurchaseApplication(
            Id id,
            IReadOnlyList<Product> products, 
            Client client, 
            Option<AdditionalInformation> additionalInformation, 
            DateTime creationDateTime)
        {
            Id = id;
            Products = products;
            Client = client;
            AdditionalInformation = additionalInformation;
            CreationDateTime = creationDateTime;
        }
    }
}