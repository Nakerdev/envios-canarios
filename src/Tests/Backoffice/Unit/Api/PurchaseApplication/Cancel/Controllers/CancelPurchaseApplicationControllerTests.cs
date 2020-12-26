using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Controllers;
using CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Repositories;
using CanaryDeliveries.Backoffice.Api.Utils;
using CanaryDeliveries.PurchaseApplication.Domain.Cancel;
using CanaryDeliveries.PurchaseApplication.Domain.Services;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using CanaryDeliveries.Tests.PurchaseApplication.Unit.Builders;
using FluentAssertions;
using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.Backoffice.Unit.Api.PurchaseApplication.Cancel.Controllers
{
    [TestFixture]
    public class CancelPurchaseApplicationControllerTests
    {
        private Mock<CancelPurchaseApplicationCommandHandler> commandHandler;
        private CancelPurchaseApplicationController controller;

        [SetUp]
        public void SetUp()
        {
            commandHandler = new Mock<CancelPurchaseApplicationCommandHandler>(
                It.IsAny<PurchaseApplicationRepository>(),
                It.IsAny<TimeService>());
            controller = new CancelPurchaseApplicationController(
                commandHandler: commandHandler.Object);
        }

        [Test]
        public void CancelsPurchaseApplication()
        {
            var request = new CancelPurchaseApplicationController.RequestDto
            {
                PurchaseApplicationId = "b5cd78a5-2e26-498a-a399-2c5cb2bf0f54",
                RejectionReason = "Razón del rechazo"
            };
            commandHandler
                .Setup(x => x.Cancel(It.IsAny<CancelPurchaseApplicationCommand>()))
                .Returns(PurchaseApplicationBuilder.Build());
            
            var response = controller.Cancel(request) as StatusCodeResult;

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            commandHandler
                .Verify(x => x.Cancel(It.Is<CancelPurchaseApplicationCommand>(y =>
                    y.PurchaseApplicationId == Id.Create(request.PurchaseApplicationId).IfFail(() => null)
                    && y.RejectionReason == RejectionReason.Create(request.RejectionReason).IfFail(() => null))), Times.Once);
        }
    }

    public sealed class CancelPurchaseApplicationController 
    {
        private readonly CancelPurchaseApplicationCommandHandler commandHandler;

        public CancelPurchaseApplicationController(CancelPurchaseApplicationCommandHandler commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        public ActionResult Cancel(RequestDto request)
        {
            var command = CancelPurchaseApplicationCommand.Create(new CancelPurchaseApplicationCommand.Dto(
                purchaseApplicationId: request.PurchaseApplicationId,
                rejectionReason: request.RejectionReason))
                .IfFail(() => throw new NotImplementedException());
            return commandHandler
                .Cancel(command)
                .Match(
                    Left: _ => throw new NotImplementedException(),
                    Right: _ => new OkResult());
        }

        public sealed class RequestDto
        {
            public string PurchaseApplicationId { get; set; }
            public string RejectionReason { get; set; }
        }
    }
}