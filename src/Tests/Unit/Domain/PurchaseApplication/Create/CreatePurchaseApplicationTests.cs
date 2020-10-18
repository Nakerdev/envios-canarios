using System;
using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.Domain.PurchaseApplication;
using CanaryDeliveries.Domain.PurchaseApplication.Create;
using CanaryDeliveries.Domain.PurchaseApplication.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.Create
{
    [TestFixture]
    public sealed class CreatePurchaseApplicationTests
    {
        private Mock<PurchaseApplicationRepository> purchaseApplicationRepository;
        private Mock<TimeService> timeService;
        private CreatePurchaseApplication command;
        
        [SetUp]
        public void SetUp()
        {
            purchaseApplicationRepository = new Mock<PurchaseApplicationRepository>();
            timeService = new Mock<TimeService>();
            command = new CreatePurchaseApplication(
                purchaseApplicationRepository: purchaseApplicationRepository.Object,
                timeService: timeService.Object);
        }

        [Test]
        public void CreatesPurchaseApplication()
        {
            var purchaseApplicationCreationRequest = BuildPurchaseApplicationRequest();
            var now = new DateTime(2020, 10, 10);
            timeService
                .Setup(x => x.UtcNow())
                .Returns(now);
            
            var createdPurchaseApplication = command.Create(purchaseApplicationCreationRequest);
            
            createdPurchaseApplication.Id.Should().NotBeNull();
            createdPurchaseApplication.Products.Count.Should().Be(purchaseApplicationCreationRequest.Products.Count);
            createdPurchaseApplication.Products.First().Link.Should().Be(purchaseApplicationCreationRequest.Products.First().Link);
            createdPurchaseApplication.Products.First().Units.Should().Be(purchaseApplicationCreationRequest.Products.First().Units);
            createdPurchaseApplication.Products.First().AdditionalInformation.Equals(purchaseApplicationCreationRequest.Products.First().AdditionalInformation).Should().BeTrue();
            createdPurchaseApplication.Products.First().PromotionCode.Equals(purchaseApplicationCreationRequest.Products.First().PromotionCode).Should().BeTrue();
            createdPurchaseApplication.Client.Name.Should().Be(purchaseApplicationCreationRequest.ClientProp.Name);
            createdPurchaseApplication.Client.PhoneNumber.Should().Be(purchaseApplicationCreationRequest.ClientProp.PhoneNumber);
            createdPurchaseApplication.Client.Email.Should().Be(purchaseApplicationCreationRequest.ClientProp.Email);
            createdPurchaseApplication.AdditionalInformation.Equals(purchaseApplicationCreationRequest.AdditionalInformation).Should().BeTrue();
            createdPurchaseApplication.CreationDateTime.Should().Be(now);
            purchaseApplicationRepository
                .Verify(x => x.Create(It.Is<CanaryDeliveries.Domain.PurchaseApplication.PurchaseApplication>(y => 
                    y.Id != null
                    && y.Products.Count == purchaseApplicationCreationRequest.Products.Count
                    && y.Products.First().Link == purchaseApplicationCreationRequest.Products.First().Link
                    && y.Products.First().Units== purchaseApplicationCreationRequest.Products.First().Units
                    && y.Products.First().AdditionalInformation == purchaseApplicationCreationRequest.Products.First().AdditionalInformation
                    && y.Products.First().PromotionCode == purchaseApplicationCreationRequest.Products.First().PromotionCode
                    && y.Client.Name== purchaseApplicationCreationRequest.ClientProp.Name
                    && y.Client.PhoneNumber== purchaseApplicationCreationRequest.ClientProp.PhoneNumber
                    && y.Client.Email == purchaseApplicationCreationRequest.ClientProp.Email
                    && y.AdditionalInformation == purchaseApplicationCreationRequest.AdditionalInformation
                    && y.CreationDateTime == now)), Times.Once);
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