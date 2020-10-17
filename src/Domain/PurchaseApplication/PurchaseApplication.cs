using System;
using System.Collections.Generic;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication
{
    public sealed class PurchaseApplication
    {
        public IReadOnlyList<Product> Products { get; }
        public Client Client { get; }
        public Option<AdditionalInformation> AdditionalInformation { get; }
        public DateTime CreationDateTime { get; }

        public PurchaseApplication(
            IReadOnlyList<Product> products, 
            Client client, 
            Option<AdditionalInformation> additionalInformation, 
            DateTime creationDateTime)
        {
            Products = products;
            Client = client;
            AdditionalInformation = additionalInformation;
            CreationDateTime = creationDateTime;
        }
    }

    public sealed class Product
    {
        public Link Link { get; }
        public Units Units { get; }
        public Option<AdditionalInformation> AdditionalInformation { get; }
        public Option<PromotionCode> PromotionCode { get; }

        public Product(
            Link link, 
            Units units, 
            Option<AdditionalInformation> additionalInformation, 
            Option<PromotionCode> promotionCode)
        {
            Link = link;
            Units = units;
            AdditionalInformation = additionalInformation;
            PromotionCode = promotionCode;
        }
    }
    
    public sealed class Client
    {
        public Name Name { get; }
        public PhoneNumber PhoneNumber { get; }
        public Email Email { get; }

        public Client(
            Name name, 
            PhoneNumber phoneNumber, 
            Email email)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
        }
    }
}