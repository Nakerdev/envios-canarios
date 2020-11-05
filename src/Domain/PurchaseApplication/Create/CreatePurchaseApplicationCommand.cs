using System;
using System.Collections.Generic;
using CanaryDeliveries.Domain.PurchaseApplication.Entities;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.Create
{
    public sealed class CreatePurchaseApplicationCommand
    {
        public IReadOnlyList<Product> Products { get; }
        public Client ClientProp { get; }
        public Option<AdditionalInformation> AdditionalInformation { get; }

        public static Validation<
            ValidationError<GenericValidationErrorCode>, 
            CreatePurchaseApplicationCommand> Create(Dto commandDto)
        {
            var products = Product.Create(commandDto.Products);
            var client = Client.Create(commandDto.Client);
            var additionalInformation = commandDto.AdditionalInformation.Map(x => Domain.PurchaseApplication.ValueObjects.AdditionalInformation.Create(x));
            
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

            return new CreatePurchaseApplicationCommand(
                products: products.IfFail(() => throw new InvalidOperationException()),
                clientProp: client.IfFail(() => throw new InvalidOperationException()),
                additionalInformation: additionalInformation.Match(
                    None: () => null, 
                    Some: x => x.IfFail(() => throw new InvalidOperationException())));
        }

        private CreatePurchaseApplicationCommand(
            IReadOnlyList<Product> products, 
            Client clientProp, 
            Option<AdditionalInformation> additionalInformation)
        {
            Products = products;
            ClientProp = clientProp;
            AdditionalInformation = additionalInformation;
        }
        
        public sealed class Dto
        {
            public List<Product.Dto> Products { get; }
            public Client.Dto Client { get; }
            public Option<string> AdditionalInformation { get; }
    
            public Dto(
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
    
}