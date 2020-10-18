using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.Domain.PurchaseApplication;
using CanaryDeliveries.Domain.PurchaseApplication.Create;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.Create
{
    [TestFixture]
    public sealed class CreatePurchaseApplicationTests
    {
        private Mock<PurchaseApplicationRepository> purchaseApplicationRepository;
        private CreatePurchaseApplication command;
        
        [SetUp]
        public void SetUp()
        {
            purchaseApplicationRepository = new Mock<PurchaseApplicationRepository>();
            command = new CreatePurchaseApplication(
                purchaseApplicationRepository: purchaseApplicationRepository.Object);
        }

        [Test]
        public void CreatesPurchaseApplication()
        {
            var purchaseApplicationCreationRequest = BuildPurchaseApplicationRequest();
            
            var createdPurchaseApplication = command.Create(purchaseApplicationCreationRequest);
            
            createdPurchaseApplication.Id.Should().NotBeNull();
            createdPurchaseApplication.Products.Count.Should().Be(purchaseApplicationCreationRequest.Products.Count);
            createdPurchaseApplication.Products.First().Link.Value.Should().Be(purchaseApplicationCreationRequest.Products.First().Link.Value);
            createdPurchaseApplication.Products.First().Units.Value.Should().Be(purchaseApplicationCreationRequest.Products.First().Units.Value);
            createdPurchaseApplication.Products.First().AdditionalInformation.Equals(purchaseApplicationCreationRequest.Products.First().AdditionalInformation).Should().BeTrue();
            createdPurchaseApplication.Products.First().PromotionCode.Equals(purchaseApplicationCreationRequest.Products.First().PromotionCode).Should().BeTrue();
            createdPurchaseApplication.Client.Name.Value.Should().Be(purchaseApplicationCreationRequest.ClientProp.Name.Value);
            createdPurchaseApplication.Client.PhoneNumber.Value.Should().Be(purchaseApplicationCreationRequest.ClientProp.PhoneNumber.Value);
            createdPurchaseApplication.Client.Email.Value.Should().Be(purchaseApplicationCreationRequest.ClientProp.Email.Value);
            createdPurchaseApplication.AdditionalInformation.Equals(purchaseApplicationCreationRequest.AdditionalInformation).Should().BeTrue();
            purchaseApplicationRepository
                .Verify(x => x.Create(It.Is<CanaryDeliveries.Domain.PurchaseApplication.PurchaseApplication>(y => 
                    y.Id != null
                    && y.Products.Count == purchaseApplicationCreationRequest.Products.Count
                    && y.Products.First().Link.Value == purchaseApplicationCreationRequest.Products.First().Link.Value
                    && y.Products.First().Units.Value == purchaseApplicationCreationRequest.Products.First().Units.Value
                    && y.Products.First().AdditionalInformation == purchaseApplicationCreationRequest.Products.First().AdditionalInformation
                    && y.Products.First().PromotionCode == purchaseApplicationCreationRequest.Products.First().PromotionCode
                    && y.Client.Name.Value == purchaseApplicationCreationRequest.ClientProp.Name.Value
                    && y.Client.PhoneNumber.Value == purchaseApplicationCreationRequest.ClientProp.PhoneNumber.Value
                    && y.Client.Email.Value == purchaseApplicationCreationRequest.ClientProp.Email.Value
                    && y.AdditionalInformation == purchaseApplicationCreationRequest.AdditionalInformation)), Times.Once);
        }

        private static PurchaseApplicationCreationRequest BuildPurchaseApplicationRequest()
        {
            var requestDto = new PurchaseApplicationCreationRequestDto(
                products: new List<PurchaseApplicationCreationRequestDto.ProductDto>
                {
                    new PurchaseApplicationCreationRequestDto.ProductDto(
                        link: "https://addidas.com/any/product",
                        units: "1",
                        additionalInformation: "Product additional product",
                        promotionCode: "ADDIDAS-123")
                },
                client: new PurchaseApplicationCreationRequestDto.ClientDto(
                    name: "Alfredo",
                    phoneNumber: "123123123",
                    email: "alfredo@elguapo.com"),
                additionalInformation: "Purchase application additional information");
            return PurchaseApplicationCreationRequest.Create(requestDto);
        }
    }
}