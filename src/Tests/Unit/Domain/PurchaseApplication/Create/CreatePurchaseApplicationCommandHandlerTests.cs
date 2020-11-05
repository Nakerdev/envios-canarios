using System;
using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.Domain.PurchaseApplication;
using CanaryDeliveries.Domain.PurchaseApplication.Create;
using CanaryDeliveries.Domain.PurchaseApplication.Entities;
using CanaryDeliveries.Domain.PurchaseApplication.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.Create
{
    [TestFixture]
    public sealed class CreatePurchaseApplicationCommandHandlerTests
    {
        private Mock<PurchaseApplicationRepository> purchaseApplicationRepository;
        private Mock<TimeService> timeService;
        private CreatePurchaseApplicationCommandHandler commandHandler;
        
        [SetUp]
        public void SetUp()
        {
            purchaseApplicationRepository = new Mock<PurchaseApplicationRepository>();
            timeService = new Mock<TimeService>();
            commandHandler = new CreatePurchaseApplicationCommandHandler(
                purchaseApplicationRepository: purchaseApplicationRepository.Object,
                timeService: timeService.Object);
        }

        [Test]
        public void CreatesPurchaseApplication()
        {
            var command = BuildCreatePurchaseApplicationCommand();
            var now = new DateTime(2020, 10, 10);
            timeService
                .Setup(x => x.UtcNow())
                .Returns(now);
            
            var createdPurchaseApplication = commandHandler.Create(command);
            
            createdPurchaseApplication.Id.Should().NotBeNull();
            createdPurchaseApplication.Products.Count.Should().Be(command.Products.Count);
            createdPurchaseApplication.Products.First().Link.Should().Be(command.Products.First().Link);
            createdPurchaseApplication.Products.First().Units.Should().Be(command.Products.First().Units);
            createdPurchaseApplication.Products.First().AdditionalInformation.Equals(command.Products.First().AdditionalInformation).Should().BeTrue();
            createdPurchaseApplication.Products.First().PromotionCode.Equals(command.Products.First().PromotionCode).Should().BeTrue();
            createdPurchaseApplication.Client.Name.Should().Be(command.ClientProp.Name);
            createdPurchaseApplication.Client.PhoneNumber.Should().Be(command.ClientProp.PhoneNumber);
            createdPurchaseApplication.Client.Email.Should().Be(command.ClientProp.Email);
            createdPurchaseApplication.AdditionalInformation.Equals(command.AdditionalInformation).Should().BeTrue();
            createdPurchaseApplication.CreationDateTime.Should().Be(now);
            purchaseApplicationRepository
                .Verify(x => x.Create(It.Is<CanaryDeliveries.Domain.PurchaseApplication.PurchaseApplication>(y => 
                    y.Id != null
                    && y.Products.Count == command.Products.Count
                    && y.Products.First().Link == command.Products.First().Link
                    && y.Products.First().Units== command.Products.First().Units
                    && y.Products.First().AdditionalInformation == command.Products.First().AdditionalInformation
                    && y.Products.First().PromotionCode == command.Products.First().PromotionCode
                    && y.Client.Name== command.ClientProp.Name
                    && y.Client.PhoneNumber== command.ClientProp.PhoneNumber
                    && y.Client.Email == command.ClientProp.Email
                    && y.AdditionalInformation == command.AdditionalInformation
                    && y.CreationDateTime == now)), Times.Once);
        }

        private static CreatePurchaseApplicationCommand BuildCreatePurchaseApplicationCommand()
        {
            var requestDto = new CreatePurchaseApplicationCommand.Dto(
                products: new List<Product.Dto>
                {
                    new Product.Dto(
                        link: "https://addidas.com/any/product",
                        units: "1",
                        additionalInformation: "Product additional product",
                        promotionCode: "ADDIDAS-123")
                },
                client: new Client.Dto(
                    name: "Alfredo",
                    phoneNumber: "123123123",
                    email: "alfredo@elguapo.com"),
                additionalInformation: "Purchase application additional information");
            return CreatePurchaseApplicationCommand
                .Create(requestDto)
                .IfFail(() => throw new InvalidOperationException());
        }
    }
}