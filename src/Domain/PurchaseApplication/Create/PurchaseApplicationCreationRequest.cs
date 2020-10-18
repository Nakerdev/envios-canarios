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

        public static PurchaseApplicationCreationRequest Create(PurchaseApplicationRequestDto requestDto)
        {
            return new PurchaseApplicationCreationRequest(
                products: requestDto.Products.Map(product => new Product(
                    link: new Link(product.Link.ValueUnsafe()),
                    units: new Units(int.Parse(product.Units.ValueUnsafe())),
                    additionalInformation: product.AdditionalInformation.Map(x => new AdditionalInformation(x)),
                    promotionCode: product.PromotionCode.Map(x => new PromotionCode(x)))).ToList().AsReadOnly(),
                clientProp: new Client(
                    name: new Name(requestDto.Client.Name.ValueUnsafe()),
                    phoneNumber: new PhoneNumber(requestDto.Client.PhoneNumber.ValueUnsafe()),
                    email: new Email(requestDto.Client.Email.ValueUnsafe())),
                additionalInformation: requestDto.AdditionalInformation.Map(x => new AdditionalInformation(x)));
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
    
    public sealed class PurchaseApplicationRequestDto
    {
        public List<ProductDto> Products { get; }
        public ClientDto Client { get; }
        public Option<string> AdditionalInformation { get; }

        public PurchaseApplicationRequestDto(
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