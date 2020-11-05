using System;
using System.Collections.Generic;
using CanaryDeliveries.Domain.PurchaseApplication.Entities;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace CanaryDeliveries.Domain.PurchaseApplication.Create
{
    public sealed class PurchaseApplicationCreationRequest
    {
        public IReadOnlyList<Product> Products { get; }
        public Client ClientProp { get; }
        public Option<AdditionalInformation> AdditionalInformation { get; }

        public static Validation<
            ValidationError<GenericValidationErrorCode>, 
            PurchaseApplicationCreationRequest> Create(PurchaseApplicationCreationRequestDto creationRequestDto)
        {
            var products = Product.Create(creationRequestDto.Products);
            var client = Client.Create(creationRequestDto.Client);
            var additionalInformation = creationRequestDto.AdditionalInformation.Map(x => Domain.PurchaseApplication.ValueObjects.AdditionalInformation.Create(x));
            
            if (products.IsFail 
                || client.IsFail 
                || additionalInformation.Match(None: () => false, Some: x => x.IsFail)
)
            {
                var validationErrors = Prelude.Seq<ValidationError<GenericValidationErrorCode>>();
                products.IfFail(errors => validationErrors = validationErrors.Concat(errors));
                client.IfFail(errors => validationErrors = validationErrors.Concat(errors));
                additionalInformation.IfSome(result => result.IfFail(errors => validationErrors = validationErrors.Concat(errors)));
                return validationErrors;
            }

            return new PurchaseApplicationCreationRequest(
                products: products.IfFail(() => throw new InvalidOperationException()),
                clientProp: client.IfFail(() => throw new InvalidOperationException()),
                additionalInformation: additionalInformation.Match(
                    None: () => null, 
                    Some: x => x.IfFail(() => throw new InvalidOperationException())));
        }

        private PurchaseApplicationCreationRequest(
            IReadOnlyList<Product> products, 
            Client clientProp, 
            Option<AdditionalInformation> additionalInformation)
        {
            Products = products;
            ClientProp = clientProp;
            AdditionalInformation = additionalInformation;
        }
    }
    
    public sealed class PurchaseApplicationCreationRequestDto
    {
        public List<Product.Dto> Products { get; }
        public Client.Dto Client { get; }
        public Option<string> AdditionalInformation { get; }

        public PurchaseApplicationCreationRequestDto(
            List<Product.Dto> products, 
            Client.Dto client, 
            Option<string> additionalInformation)
        {
            Products = products;
            Client = client;
            AdditionalInformation = additionalInformation;
        }
    }
}