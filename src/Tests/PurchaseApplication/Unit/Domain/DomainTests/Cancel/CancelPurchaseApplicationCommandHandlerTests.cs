using System;
using CanaryDeliveries.PurchaseApplication.Domain;
using CanaryDeliveries.PurchaseApplication.Domain.Cancel;
using CanaryDeliveries.PurchaseApplication.Domain.Services;
using CanaryDeliveries.Tests.PurchaseApplication.Unit.Builders;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.PurchaseApplication.Unit.DomainTests.Cancel
{
    [TestFixture]
    public sealed class CancelPurchaseApplicationCommandHandlerTests
    {
        private Mock<PurchaseApplicationRepository> purchaseApplicationRepository;
        private Mock<TimeService> timeService;
        private CancelPurchaseApplicationCommandHandler commandHandler;
        
        [SetUp]
        public void SetUp()
        {
            purchaseApplicationRepository = new Mock<PurchaseApplicationRepository>();
            timeService = new Mock<TimeService>();
            commandHandler = new CancelPurchaseApplicationCommandHandler(
                purchaseApplicationRepository: purchaseApplicationRepository.Object,
                timeService: timeService.Object);
        }

        [Test]
        public void CancelsPurchaseApplication()
        {
            var command = BuildCancelPurchaseApplicationCommand();
            var purchaseApplication = PurchaseApplicationBuilder.Build(id: command.Id.State.Value);
            purchaseApplicationRepository
                .Setup(x => x.SearchBy(command.Id))
                .Returns(purchaseApplication);
            var utcNow = new DateTime(2020, 10, 10, 12, 30, 00);
            timeService
                .Setup(x => x.UtcNow())
                .Returns(utcNow);
            
            var cancelledPurchaseApplication = commandHandler.Cancel(command);

            cancelledPurchaseApplication.IsRight.Should().BeTrue();
            purchaseApplicationRepository
                .Verify(x => x.Update(It.Is<CanaryDeliveries.PurchaseApplication.Domain.PurchaseApplication>(y => 
                    y.Id == command.Id
                    && y.RejectionDateTime == utcNow
                    && y.RejectionReason == command.RejectionReason
                    && y.State == State.Rejected)), Times.Once);
        }

        private static CancelPurchaseApplicationCommand BuildCancelPurchaseApplicationCommand()
        {
            return CancelPurchaseApplicationCommand.Create(new CancelPurchaseApplicationCommand.Dto(
                    id: "b5cd78a5-2e26-498a-a399-2c5cb2bf0f54",
                    rejectionReason: "El usuario esta itentando comprar productos de falsificacion"))
                .IfFail(() => throw new InvalidOperationException());
        }
    }
}