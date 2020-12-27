using CanaryDeliveries.Backoffice.Api.PurchaseApplication.Cancel.Controllers;
using CanaryDeliveries.Backoffice.Api.PurchaseApplication.Search.Repositories;
using CanaryDeliveries.Backoffice.Api.Utils;
using CanaryDeliveries.PurchaseApplication.Domain.Cancel;
using CanaryDeliveries.PurchaseApplication.Domain.Services;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using CanaryDeliveries.Tests.PurchaseApplication.Unit.Builders;
using FluentAssertions;
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
            var request = BuildRequest();
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
        
        [Test]
        public void DoesNotCancelPurchaseApplicationWhenCommandHandlerFails()
        {
            var error = Error.PurchaseApplicationNotFound;
            commandHandler
                .Setup(x => x.Cancel(It.IsAny<CancelPurchaseApplicationCommand>()))
                .Returns(error);

            var response = controller.Cancel(BuildRequest()) as ObjectResult;

            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            var badRequestResponseModel = (BadRequestResponseModel) response.Value;
            badRequestResponseModel.ValidationErrors.Should().BeNull();
            badRequestResponseModel.OperationError.Should().Be(error.ToString());
        }
        
        [Test]
        public void DoesNotCancelPurchaseApplicationWhenCommandCreationHasValidationErrors()
        {
            var request = BuildRequest(purchaseApplicationId: null);

            var response = controller.Cancel(request) as ObjectResult;

            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            var badRequestResponseModel = (BadRequestResponseModel) response.Value;
            badRequestResponseModel.ValidationErrors.Should().NotBeNull();
            badRequestResponseModel.OperationError.Should().BeNull();
            commandHandler
                .Verify(x => x.Cancel(It.Is<CancelPurchaseApplicationCommand>(y =>
                    y.PurchaseApplicationId == Id.Create(request.PurchaseApplicationId).IfFail(() => null)
                    && y.RejectionReason == RejectionReason.Create(request.RejectionReason).IfFail(() => null))), Times.Never);
        }

        private static CancelPurchaseApplicationController.CancelRequestDto BuildRequest(
            string purchaseApplicationId = "b5cd78a5-2e26-498a-a399-2c5cb2bf0f54")
        {
            return new CancelPurchaseApplicationController.CancelRequestDto
            {
                PurchaseApplicationId = purchaseApplicationId,
                RejectionReason = "Razón del rechazo"
            };
        }
    }
}