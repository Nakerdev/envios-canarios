using System.Linq;
using CanaryDeliveries.PurchaseApplication.Domain.Cancel;
using CanaryDeliveries.PurchaseApplication.Domain.Create;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.PurchaseApplication.Unit.DomainTests.Cancel
{
    [TestFixture]
    public sealed class CreatePurchaseApplicationCommandTests
    {
        [Test]
        public void CreatesCommand()
        {
            var commandDto = BuildCancelPurchaseApplicationCommandDto();

            var result = CancelPurchaseApplicationCommand.Create(commandDto);

            result.IsSuccess.Should().BeTrue();
            result.IfSuccess(command =>
            {
                var expectedPurchaseApplicationId = Id.Create(commandDto.PurchaseApplicationId).IfFail(() => null);
                command.PurchaseApplicationId.Should().Be(expectedPurchaseApplicationId);
                var expectedRejectionReason = RejectionReason.Create(commandDto.RejectionReason).IfFail(() => null);
                command.RejectionReason.Should().Be(expectedRejectionReason);
            });
        }

        [Test]
        public void DoesNotCreateCommandWhenPurchaseApplicationIdIsNotValid()
        {
            var commandDto = BuildCancelPurchaseApplicationCommandDto(purchaseApplicationId: null);

            var result = CancelPurchaseApplicationCommand.Create(commandDto);

            result.IsFail.Should().BeTrue();
            result.IfFail(error => error.First().FieldId.Should().Be(nameof(CancelPurchaseApplicationCommand.PurchaseApplicationId)));
        }
        
        [Test]
        public void DoesNotCreateCommandWhenRejectionReasonIsNotValid()
        {
            var commandDto = BuildCancelPurchaseApplicationCommandDto(rejectionReason: null);

            var result = CancelPurchaseApplicationCommand.Create(commandDto);

            result.IsFail.Should().BeTrue();
        }

        private static CancelPurchaseApplicationCommand.Dto BuildCancelPurchaseApplicationCommandDto(
            string purchaseApplicationId = "b5cd78a5-2e26-498a-a399-2c5cb2bf0f54",
            string rejectionReason = "Razon del rechazo")
        {
            return new CancelPurchaseApplicationCommand.Dto(
                purchaseApplicationId: purchaseApplicationId,
                rejectionReason: rejectionReason);
        }
    }
}