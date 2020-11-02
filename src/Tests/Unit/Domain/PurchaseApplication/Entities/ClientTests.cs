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
            var clientDto = new Client.ClientDto(
                name: "Alfredo", 
                phoneNumber: "610929292", 
                email: "alfredo@email.com");
            
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
    }
}