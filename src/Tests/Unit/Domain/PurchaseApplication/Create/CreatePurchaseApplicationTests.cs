using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using FluentAssertions;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Moq;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.Create
{
    [TestFixture]
    public sealed class CreatePurchaseApplicationTests
    {
        [Test]
        public void CreatesPurchaseApplication()
        {
            var purchaseApplicationRepository = new Mock<PurchaseApplicationRepository>();
            var command = new CreatePurchaseApplication(
                purchaseApplicationRepository: purchaseApplicationRepository.Object);
            var requestDto = new PurchaseApplicationRequestDto(
                products: new List<PurchaseApplicationRequestDto.ProductDto>
                {
                    new PurchaseApplicationRequestDto.ProductDto(
                        link: "https://addidas.com/any/product",
                        units: "1",
                        additionalInformation: "Product additional product",
                        promotionCode: "ADDIDAS-123")
                },
                client: new PurchaseApplicationRequestDto.ClientDto(
                    name: "Alfredo",
                    phoneNumber: "123123123",
                    email: "alfredo@elguapo.com"),
                additionalInformation: "Purchase application additional information");
            var purchaseApplicationCreationRequest = PurchaseApplicationCreationRequest.Create(requestDto);
            
            var createdPurchaseApplication = command.Create(purchaseApplicationCreationRequest);
            
            createdPurchaseApplication.Id.Should().NotBeNull();
            createdPurchaseApplication.Products.Count.Should().Be(requestDto.Products.Count);
            createdPurchaseApplication.Products.First().Link.Value.Should().Be(requestDto.Products.First().Link.ValueUnsafe());
            createdPurchaseApplication.Products.First().Units.Value.Should().Be(int.Parse(requestDto.Products.First().Units.ValueUnsafe()));
            createdPurchaseApplication.Products.First().AdditionalInformation.ValueUnsafe().Value.Should().Be(requestDto.Products.First().AdditionalInformation.ValueUnsafe());
            createdPurchaseApplication.Products.First().PromotionCode.ValueUnsafe().Value.Should().Be(requestDto.Products.First().PromotionCode.ValueUnsafe());
            createdPurchaseApplication.Client.Name.Value.Should().Be(requestDto.Client.Name.ValueUnsafe());
            createdPurchaseApplication.Client.PhoneNumber.Value.Should().Be(requestDto.Client.PhoneNumber.ValueUnsafe());
            createdPurchaseApplication.Client.Email.Value.Should().Be(requestDto.Client.Email.ValueUnsafe());
            createdPurchaseApplication.AdditionalInformation.ValueUnsafe().Value.Should().Be(requestDto.AdditionalInformation.ValueUnsafe());
            purchaseApplicationRepository
                .Verify(x => x.Create(It.Is<CanaryDeliveries.Domain.PurchaseApplication.PurchaseApplication>(y => 
                    y.Id != null
                    && y.Products.Count == requestDto.Products.Count
                    && y.Products.First().Link.Value == requestDto.Products.First().Link.ValueUnsafe()
                    && y.Products.First().Units.Value == int.Parse(requestDto.Products.First().Units.ValueUnsafe())
                    && y.Products.First().AdditionalInformation.ValueUnsafe().Value == requestDto.Products.First().AdditionalInformation.ValueUnsafe()
                    && y.Products.First().PromotionCode.ValueUnsafe().Value == requestDto.Products.First().PromotionCode.ValueUnsafe()
                    && y.Client.Name.Value == requestDto.Client.Name.ValueUnsafe()
                    && y.Client.PhoneNumber.Value == requestDto.Client.PhoneNumber.ValueUnsafe()
                    && y.Client.Email.Value == requestDto.Client.Email.ValueUnsafe()
                    && y.AdditionalInformation.ValueUnsafe().Value == requestDto.AdditionalInformation.ValueUnsafe())), Times.Once);
        }
    }

    public interface PurchaseApplicationRepository
    {
        void Create(CanaryDeliveries.Domain.PurchaseApplication.PurchaseApplication purchaseApplication);
    }

    public sealed class CreatePurchaseApplication
    {
        private readonly PurchaseApplicationRepository _purchaseApplicationRepository;

        public CreatePurchaseApplication(PurchaseApplicationRepository purchaseApplicationRepository)
        {
            _purchaseApplicationRepository = purchaseApplicationRepository;
        }

        public CanaryDeliveries.Domain.PurchaseApplication.PurchaseApplication Create(PurchaseApplicationCreationRequest purchaseApplicationCreationRequest)
        {
            throw new System.NotImplementedException();
        }
    }

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