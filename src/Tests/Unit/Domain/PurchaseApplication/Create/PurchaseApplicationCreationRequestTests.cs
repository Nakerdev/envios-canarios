using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.Domain.PurchaseApplication.Create;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using FluentAssertions;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.Create
{
    [TestFixture]
    public sealed class PurchaseApplicationCreationRequestTests
    {
        [Test]
        public void CreatesPurchaseApplicationCreationRequest()
        {
            var requestDto = BuildPurchaseApplicationCreationRequestDto();

            var request = PurchaseApplicationCreationRequest.Create(requestDto);
            
            request.Products.Count.Should().Be(requestDto.Products.Count);
            var expectedLink = Link.Create(requestDto.Products.First().Link);
            request.Products.First().Link.Should().Be(expectedLink.ValueUnsafe());
            var expectedUnits = Units.Create(requestDto.Products.First().Units);
            request.Products.First().Units.Should().Be(expectedUnits.ValueUnsafe());
            request.Products.First().AdditionalInformation.IsSome.Should().BeTrue();
            var expectedAdditionalInformation = AdditionalInformation.Create(requestDto.Products.First().AdditionalInformation);
            request.Products.First().AdditionalInformation.IfSome(x => x.Should().Be(expectedAdditionalInformation.ValueUnsafe()));
            request.Products.First().PromotionCode.IsSome.Should().BeTrue();
            var expectedPromotionCode = PromotionCode.Create(requestDto.Products.First().PromotionCode);
            request.Products.First().PromotionCode.IfSome(x => x.Should().Be(expectedPromotionCode.ValueUnsafe()));
            var expectedClientName = Name.Create(requestDto.Client.Email);
            request.ClientProp.Name.Should().Be(expectedClientName.ValueUnsafe());
            var expectedClientPhoneNumber = PhoneNumber.Create(requestDto.Client.PhoneNumber);
            request.ClientProp.PhoneNumber.Should().Be(expectedClientPhoneNumber.ValueUnsafe());
            var expectedClientEmail = Email.Create(requestDto.Client.Email);
            request.ClientProp.Email.Should().Be(expectedClientEmail.ValueUnsafe());
            request.AdditionalInformation.IsSome.Should().BeTrue();
            var expectedPurchaseApplicationAdditionalInformation = AdditionalInformation.Create(requestDto.AdditionalInformation);
            request.AdditionalInformation.IfSome(x => x.Should().Be(expectedPurchaseApplicationAdditionalInformation .ValueUnsafe()));
        }
        

        private static PurchaseApplicationCreationRequestDto BuildPurchaseApplicationCreationRequestDto ()
        {
            return new PurchaseApplicationCreationRequestDto(
                products: new List<PurchaseApplicationCreationRequestDto.ProductDto>
                {
                    new PurchaseApplicationCreationRequestDto.ProductDto(
                        link: "https://addidas.com/any/product",
                        units: "1",
                        additionalInformation: "Product additional product",
                        promotionCode: "ADDIDAS-123")
                },
                client: new PurchaseApplicationCreationRequestDto.ClientDto(
                    name: "Alfredo",
                    phoneNumber: "123123123",
                    email: "alfredo@elguapo.com"),
                additionalInformation: "Purchase application additional information");
        }
    }
}