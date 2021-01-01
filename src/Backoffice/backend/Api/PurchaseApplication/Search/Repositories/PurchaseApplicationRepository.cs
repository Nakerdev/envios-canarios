using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CanaryDeliveries.PurchaseApplication.Domain;

namespace CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Repositories
{
    public interface PurchaseApplicationRepository
    {
        ReadOnlyCollection<PurchaseApplicationDto> SearchAll();
    }

    public sealed class PurchaseApplicationDto
    { 
        public string Id { get; }
        public ReadOnlyCollection<ProductDto> Products { get; }
        public ClientDto Client { get; }
        public string AdditionalInformation { get; }
        public DateTime CreationDateTime { get; }
        public State State { get; }

        public PurchaseApplicationDto(
            string id,
            ReadOnlyCollection<ProductDto> products, 
            ClientDto client, 
            string additionalInformation,
            DateTime creationDateTime, 
            State state)
        {
            Id = id;
            Products = products;
            Client = client;
            AdditionalInformation = additionalInformation;
            CreationDateTime = creationDateTime;
            State = state;
        }

        public sealed class ProductDto
        {
            public string Id { get; }
            public string Link { get; }
            public int Units { get; }
            public string AdditionalInformation { get; }
            public string PromotionCode { get; }

            public ProductDto(
                string id,
                string link, 
                int units, 
                string additionalInformation, 
                string promotionCode)
            {
                Id = id;
                Link = link;
                Units = units;
                AdditionalInformation = additionalInformation;
                PromotionCode = promotionCode;
            }
        }

        public sealed class ClientDto
        {
            public string Name { get; }
            public string PhoneNumber { get; }
            public string Email { get; }

            public ClientDto(
                string name, 
                string phoneNumber, 
                string email)
            {
                Name = name;
                PhoneNumber = phoneNumber;
                Email = email;
            }
        }
    }
}