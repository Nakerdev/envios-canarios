using CanaryDeliveries.Domain.PurchaseApplication.Entities;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.Entities
{
    [TestFixture]
    public sealed class ClientTests
    {
        [Test]
        public void CreatesClient()
        {
            var clientDto = buildClientDto();
            
            var result = Client.Create(clientDto);

            result.IsSuccess.Should().BeTrue();
            result.IfSuccess(client =>
            {
                var expectedName = Name.Create(clientDto.Email).IfFail(() => null);
                client.Name.Should().Be(expectedName);
                var expectedPhoneNumber = PhoneNumber.Create(clientDto.PhoneNumber).IfFail(() => null);
                client.PhoneNumber.Should().Be(expectedPhoneNumber);
                var expectedEmail = Email.Create(clientDto.Email).IfFail(() => null);
                client.Email.Should().Be(expectedEmail);
            });
        }
        
        [Test]
        public void DoesNotCreateClientWhenNameHasValidationErrors()
        {
            var clientDto = buildClientDto(name: null);
            
            var result = Client.Create(clientDto);

            result.IsFail.Should().BeTrue();
            result.IfFail(validationErrors =>
            {
                validationErrors.Count.Should().Be(1);
                validationErrors.First().FieldId.Should().Be($"{nameof(Client)}.{nameof(Name)}");
            });
        }

        private static Client.Dto buildClientDto(
            string name = "Alfredo",
            string phoneNumber = "620929292",
            string email = "alfredo@email.com")
        {
            return new Client.Dto(
                name: name,
                phoneNumber: phoneNumber,
                email: email);
        }
    }
}