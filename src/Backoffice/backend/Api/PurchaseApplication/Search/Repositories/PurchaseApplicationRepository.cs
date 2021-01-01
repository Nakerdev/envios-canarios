using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CanaryDeliveries.PurchaseApplication.Domain;
using LanguageExt;

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
        public Option<RejectionDto> Rejection { get; }

        public PurchaseApplicationDto(
            string id,
            ReadOnlyCollection<ProductDto> products, 
            ClientDto client, 
            string additionalInformation,
            DateTime creationDateTime, 
            State state, 
            Option<RejectionDto> rejection)
        {
            Id = id;
            Products = products;
            Client = client;
            AdditionalInformation = additionalInformation;
            CreationDateTime = creationDateTime;
            State = state;
            Rejection = rejection;
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

        public sealed class RejectionDto
        {
            public DateTime DateTime { get; }
            public string Reason { get; }

            public RejectionDto(DateTime dateTime, string reason)
            {
                DateTime = dateTime;
                Reason = reason;
            }
        }
    }
}