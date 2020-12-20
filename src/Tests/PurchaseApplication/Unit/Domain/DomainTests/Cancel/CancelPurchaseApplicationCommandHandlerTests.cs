using System;
using CanaryDeliveries.PurchaseApplication.Domain;
using CanaryDeliveries.PurchaseApplication.Domain.Cancel;
using CanaryDeliveries.PurchaseApplication.Domain.Services;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
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
            var utcNow = new DateTime(2020, 10, 10);
            timeService
                .Setup(x => x.UtcNow())
                .Returns(utcNow);
            
            var cancelledPurchaseApplication = commandHandler.Cancel(command);

            cancelledPurchaseApplication.IsRight.Should().BeTrue();
            purchaseApplicationRepository
                .Verify(x => x.Update(It.Is<CanaryDeliveries.PurchaseApplication.Domain.PurchaseApplication>(y => 
                    y.Id == purchaseApplication.Id
                    && y.Rejection.IsSome
                    && y.State == State.Rejected)), Times.Once);
        }
        
        [Test]
        public void DoesNotCancelPurchaseApplicationIfIsAlreadyCancelled()
        {
            purchaseApplicationRepository
                .Setup(x => x.SearchBy(It.IsAny<Id>()))
                .Returns(PurchaseApplicationBuilder.Build(isRejected: true));
            timeService
                .Setup(x => x.UtcNow())
                .Returns(new DateTime(2020, 10, 10));
            
            var cancelledPurchaseApplication = commandHandler.Cancel(BuildCancelPurchaseApplicationCommand());

            cancelledPurchaseApplication.IsLeft.Should().BeTrue();
            cancelledPurchaseApplication.IfLeft(error => 
                error.Should().Be(CanaryDeliveries.PurchaseApplication.Domain.Cancel.Error.PurchaseApplicationIsAlreadyRejected));
            purchaseApplicationRepository
                .Verify(x => x.Update(It.IsAny<CanaryDeliveries.PurchaseApplication.Domain.PurchaseApplication>()), 
                    Times.Never);
        }
        
        [Test]
        public void DoesNotCancelPurchaseApplicationIfNotFound()
        {
            purchaseApplicationRepository
                .Setup(x => x.SearchBy(It.IsAny<Id>()))
                .Returns((CanaryDeliveries.PurchaseApplication.Domain.PurchaseApplication) null);
            
            var cancelledPurchaseApplication = commandHandler.Cancel(BuildCancelPurchaseApplicationCommand());

            cancelledPurchaseApplication.IsLeft.Should().BeTrue();
            cancelledPurchaseApplication.IfLeft(error => 
                error.Should().Be(CanaryDeliveries.PurchaseApplication.Domain.Cancel.Error.PurchaseApplicationNotFound));
            purchaseApplicationRepository
                .Verify(x => x.Update(It.IsAny<CanaryDeliveries.PurchaseApplication.Domain.PurchaseApplication>()), 
                    Times.Never);
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