using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.PurchaseApplication.Domain.Create;
using CanaryDeliveries.PurchaseApplication.Domain.Entities;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using FluentAssertions;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.Create
{
    [TestFixture]
    public sealed class CreatePurchaseApplicationCommandTests
    {
        [Test]
        public void CreatesCommand()
        {
            var commandDto = BuildCreatePurchaseApplicationCommandDto();

            var result = CreatePurchaseApplicationCommand.Create(commandDto);

            result.IsSuccess.Should().BeTrue();
            result.IfSuccess(command =>
            {
                command.Products.Count.Should().Be(commandDto.Products.Count);
                var expectedLink = Link.Create(commandDto.Products.First().Link.ValueUnsafe()).IfFail(() => null);
                command.Products.First().Link.Should().Be(expectedLink);
                var expectedUnits = Units.Create(commandDto.Products.First().Units).IfFail(() => null);
                command.Products.First().Units.Should().Be(expectedUnits);
                var expectedAdditionalInformation = AdditionalInformation.Create(commandDto.Products.First().AdditionalInformation).IfFail(() => null);
                command.Products.First().AdditionalInformation.IsSome.Should().BeTrue();
                command.Products.First().AdditionalInformation.IfSome(x => x.Should().Be(expectedAdditionalInformation));
                var expectedPromotionCode = PromotionCode.Create(commandDto.Products.First().PromotionCode).IfFail(() => null);
                command.Products.First().PromotionCode.IsSome.Should().BeTrue();
                command.Products.First().PromotionCode.IfSome(x => x.Should().Be(expectedPromotionCode));
                var expectedClientName = Name.Create(commandDto.Client.Email).IfFail(() => null);
                command.ClientProp.Name.Should().Be(expectedClientName);
                var expectedClientPhoneNumber = PhoneNumber.Create(commandDto.Client.PhoneNumber).IfFail(() => null);
                command.ClientProp.PhoneNumber.Should().Be(expectedClientPhoneNumber);
                var expectedClientEmail = Email.Create(commandDto.Client.Email).IfFail(() => null);
                command.ClientProp.Email.Should().Be(expectedClientEmail);
                var expectedPurchaseApplicationAdditionalInformation = AdditionalInformation.Create(commandDto.AdditionalInformation).IfFail(() => null);
                command.AdditionalInformation.IsSome.Should().BeTrue();
                command.AdditionalInformation.IfSome(x => x.Should().Be(expectedPurchaseApplicationAdditionalInformation));    
            });
        }

        [Test]
        public void DoesNotCreateCommandWhenProductsHaveValidationErrors()
        {
            var commandDto = BuildCreatePurchaseApplicationCommandDto(isProductListEmpty: true);

            var result = CreatePurchaseApplicationCommand.Create(commandDto);

            result.IsFail.Should().BeTrue();
        }
        
        [Test]
        public void DoesNotCreateCommandWhenClientHasValidationErrors()
        {
            var commandDto = BuildCreatePurchaseApplicationCommandDto(clientName: null);

            var result = CreatePurchaseApplicationCommand.Create(commandDto);

            result.IsFail.Should().BeTrue();
        }
        
        [Test]
        public void DoesNotCreateCommandWhenAdditionalInformationHasValidationErrors()
        {
            var commandDto = BuildCreatePurchaseApplicationCommandDto(additionalInformation: new string('a', 1001));

            var result = CreatePurchaseApplicationCommand.Create(commandDto);

            result.IsFail.Should().BeTrue();
        }

        private static CreatePurchaseApplicationCommand.Dto BuildCreatePurchaseApplicationCommandDto(
            bool isProductListEmpty = false,
            string productLink = "https://addidas.com/products/1",
            string clientName = "Alfredo",
            string additionalInformation = "Purchase application additional information")
        {
            return new CreatePurchaseApplicationCommand.Dto(
                products: BuildProducts(),
                client: new Client.Dto(
                    name: clientName,
                    phoneNumber: "123123123",
                    email: "alfredo@elguapo.com"),
                additionalInformation: additionalInformation);

            List<Product.Dto> BuildProducts()
            {
                return isProductListEmpty 
                    ? new List<Product.Dto>() 
                    : new List<Product.Dto>
                    {
                        new Product.Dto(
                            link: productLink,
                            units: "1",
                            additionalInformation: "Product additional product",
                            promotionCode: "ADDIDAS-123")
                    };
            }
        }
    }
}