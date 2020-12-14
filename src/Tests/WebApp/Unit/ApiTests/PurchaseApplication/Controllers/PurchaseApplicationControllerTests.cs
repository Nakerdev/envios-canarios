using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.PurchaseApplication.Domain;
using CanaryDeliveries.PurchaseApplication.Domain.Create;
using CanaryDeliveries.PurchaseApplication.Domain.Services;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using CanaryDeliveries.Tests.PurchaseApplication.Unit.Builders;
using CanaryDeliveries.WebApp.Api.PurchaseApplication.Create.Controllers;
using CanaryDeliveries.WebApp.Api.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.WebApp.Unit.ApiTests.PurchaseApplication.Controllers
{
    public class PurchaseApplicationControllerTests
    {   
        private Mock<CreatePurchaseApplicationCommandHandler> createPurchaseApplicationCommandHandler;
        private PurchaseApplicationController controller;

        [SetUp]
        public void Setup()
        {
            createPurchaseApplicationCommandHandler = new Mock<CreatePurchaseApplicationCommandHandler>(
                It.IsAny<PurchaseApplicationRepository>(), 
                It.IsAny<TimeService>());
            controller = new PurchaseApplicationController(
                commandHandler: createPurchaseApplicationCommandHandler.Object);
        }

        [Test]
        public void CreatesPurchaseApplication()
        {
            var request = BuildPurchaseApplicationRequest();
            createPurchaseApplicationCommandHandler
                .Setup(x => x.Create(It.IsAny<CreatePurchaseApplicationCommand>()))
                .Returns(() => PurchaseApplicationBuilder.Build());

            var response = controller.Execute(request) as StatusCodeResult;

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            createPurchaseApplicationCommandHandler
                .Verify(x => x.Create(It.Is<CreatePurchaseApplicationCommand>(y =>
                    y.Products.Count == request.Products.Count
                    && y.Products.First().Link == Link.Create(request.Products.First().Link).IfFail(() => null)
                    && y.Products.First().Units == Units.Create(request.Products.First().Units).IfFail(() => null)
                    && y.Products.First().AdditionalInformation == AdditionalInformation.Create(request.Products.First().AdditionalInformation).IfFail(() => null)
                    && y.Products.First().PromotionCode == PromotionCode.Create(request.Products.First().PromotionCode).IfFail(() => null)
                    && y.ClientProp.Email == Email.Create(request.Client.Email).IfFail(() => null)
                    && y.ClientProp.PhoneNumber == PhoneNumber.Create(request.Client.PhoneNumber).IfFail(() => null)
                    && y.ClientProp.Name == Name.Create(request.Client.Name).IfFail(() => null)
                    && y.AdditionalInformation == AdditionalInformation.Create(request.AdditionalInformation).IfFail(() => null))),
                    Times.Once);
        }
        
        [Test]
        public void DoesNotCreatePurchaseApplicationWhenCommandCreationHasValidationErrors()
        {
            var request = BuildPurchaseApplicationRequest(clientName: null);

            var response = controller.Execute(request) as ObjectResult;

            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            var badRequestResponseModel = ((BadRequestResponseModel) response.Value);
            badRequestResponseModel.ValidationErrors.Should().NotBeNull();
            badRequestResponseModel.OperationError.Should().BeNull();
            createPurchaseApplicationCommandHandler
                .Verify(x => x.Create(It.IsAny<CreatePurchaseApplicationCommand>()),
                    Times.Never);
        }

        private static PurchaseApplicationController.PurchaseApplicationCreationRequest BuildPurchaseApplicationRequest(
            string clientName = "Alfredo")
        {
            return new PurchaseApplicationController.PurchaseApplicationCreationRequest
            {
                Products = new List<PurchaseApplicationController.Product>
                {
                    new PurchaseApplicationController.Product
                    {
                        Link = "https://www.addida.com/any/product",
                        Units = "1",
                        AdditionalInformation = "Additional product information",
                        PromotionCode = "ADDIDAS-123"
                    }
                },
                Client = new  PurchaseApplicationController.Client
                {
                    Name = clientName,
                    PhoneNumber = "123123123",
                    Email = "alfredo@elguapo.com"
                },
                AdditionalInformation = "Additional purchase application information"
            };
        }
    }
}