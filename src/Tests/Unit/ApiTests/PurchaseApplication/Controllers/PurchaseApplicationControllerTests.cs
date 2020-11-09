using System;
using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.PurchaseApplication.Domain;
using CanaryDeliveries.PurchaseApplication.Domain.Create;
using CanaryDeliveries.PurchaseApplication.Domain.Entities;
using CanaryDeliveries.PurchaseApplication.Domain.Services;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using CanaryDeliveries.WebApp.Api.PurchaseApplication.Controllers;
using FluentAssertions;
using LanguageExt.ClassInstances;
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
                .Returns(() => new CanaryDeliveries.PurchaseApplication.Domain.PurchaseApplication(
                    new PersistenceState(
                        id: new Id.PersistenceState("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                        products: new List<Product.PersistenceState>
                        {
                            new Product.PersistenceState(
                                link: new Link.PersistenceState("https://addidas.com/products/1"),
                                units: new Units.PersistenceState(1),
                                additionalInformation: new AdditionalInformation.PersistenceState("size: 30, color: red"),
                                promotionCode: new PromotionCode.PersistenceState("ADDIDAS-123"))
                        },
                        client: new Client.PersistenceState(
                            name: new Name.PersistenceState("Alfredo"),
                            phoneNumber: new PhoneNumber.PersistenceState("610232323"),
                            email: new Email.PersistenceState("alfredo@email.com")),
                        additionalInformation: new AdditionalInformation.PersistenceState("Purchase additional information"),
                        creationDate: new DateTime(2020, 10, 10, 12, 30, 00))));

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

        private static PurchaseApplicationController.PurchaseApplicationRequest BuildPurchaseApplicationRequest()
        {
            return new PurchaseApplicationController.PurchaseApplicationRequest
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
                    Name = "Alfredo",
                    PhoneNumber = "123123123",
                    Email = "alfredo@elguapo.com"
                },
                AdditionalInformation = "Additional purchase application information"
            };
        }
    }
}