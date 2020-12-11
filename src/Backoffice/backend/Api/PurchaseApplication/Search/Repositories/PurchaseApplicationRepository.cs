using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Repositories
{
    public interface PurchaseApplicationRepository
    {
        ReadOnlyCollection<PurchaseApplicationDto> SearchAll();
    }

    public sealed class PurchaseApplicationDto
    { 
        public List<ProductDto> Products { get; }
        public ClientDto Client { get; }
        public string AdditionalInformation { get; }
        public DateTime CreationDateTime { get; }

        public PurchaseApplicationDto(
            List<ProductDto> products, 
            ClientDto client, 
            string additionalInformation,
            DateTime creationDateTime)
        {
            Products = products;
            Client = client;
            AdditionalInformation = additionalInformation;
            CreationDateTime = creationDateTime;
        }

        public sealed class ProductDto
        {
            public string Link { get; }
            public int Units { get; }
            public string AdditionalInformation { get; }
            public string PromotionCode { get; }
    
            public ProductDto(
                string link, 
                int units, 
                string additionalInformation, 
                string promotionCode)
            {
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