using System.Collections.Generic;
using System.Linq;
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

        public static PurchaseApplicationCreationRequest Create(PurchaseApplicationCreationRequestDto creationRequestDto)
        {
            return new PurchaseApplicationCreationRequest(
                products: creationRequestDto.Products.Map(BuildProduct).ToList().AsReadOnly(),
                clientProp: BuildClient(),
                additionalInformation: creationRequestDto.AdditionalInformation.Map(value => new AdditionalInformation(value)));

            Product BuildProduct(PurchaseApplicationCreationRequestDto.ProductDto product)
            {
                return new Product(
                    link: Link.Create(product.Link).ValueUnsafe(),
                    units: Units.Create(product.Units).ValueUnsafe(),
                    additionalInformation: product.AdditionalInformation.Map(value => new AdditionalInformation(value)),
                    promotionCode: product.PromotionCode.Map(value => new PromotionCode(value)));
            }

            Client BuildClient()
            {
                return new Client(
                    name: Name.Create(creationRequestDto.Client.Name).ValueUnsafe(),
                    phoneNumber: PhoneNumber.Create(creationRequestDto.Client.PhoneNumber).ValueUnsafe(),
                    email: Email.Create(creationRequestDto.Client.Email).ValueUnsafe());
            }
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
    
    public sealed class PurchaseApplicationCreationRequestDto
    {
        public List<ProductDto> Products { get; }
        public ClientDto Client { get; }
        public Option<string> AdditionalInformation { get; }

        public PurchaseApplicationCreationRequestDto(
            List<ProductDto> products, 
            ClientDto client, 
            Option<string> additionalInformation)
        {
            Products = products;
            Client = client;
            AdditionalInformation = additionalInformation;
        }

        public sealed class ProductDto
        {
            public Option<string> Link { get; }
            public Option<string> Units { get; }
            public Option<string> AdditionalInformation { get; }
            public Option<string> PromotionCode { get; }

            public ProductDto(
                Option<string> link, 
                Option<string> units, 
                Option<string> additionalInformation, 
                Option<string> promotionCode)
            {
                Link = link;
                Units = units;
                AdditionalInformation = additionalInformation;
                PromotionCode = promotionCode;
            }
        }
        
        public sealed class ClientDto
        {
            public Option<string> Name { get; }
            public Option<string> PhoneNumber { get; }
            public Option<string> Email { get; }

            public ClientDto(
                Option<string> name, 
                Option<string> phoneNumber, 
                Option<string> email)
            {
                Name = name;
                PhoneNumber = phoneNumber;
                Email = email;
            }
        }
    }
}