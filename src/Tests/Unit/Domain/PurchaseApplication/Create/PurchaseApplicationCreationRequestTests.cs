using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.Domain.PurchaseApplication.Create;
using CanaryDeliveries.Domain.PurchaseApplication.Entities;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using FluentAssertions;
using LanguageExt.UnsafeValueAccess;
using NUnit.Framework;
using PluralizeService.Core;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.Create
{
    [TestFixture]
    public sealed class PurchaseApplicationCreationRequestTests
    {
        [Test]
        public void CreatesPurchaseApplicationCreationRequest()
        {
            var requestDto = BuildPurchaseApplicationCreationRequestDto();

            var result = PurchaseApplicationCreationRequest.Create(requestDto);

            result.IsSuccess.Should().BeTrue();
            result.IfSuccess(request =>
            {
                request.Products.Count.Should().Be(requestDto.Products.Count);
                var expectedLink = Link.Create(requestDto.Products.First().Link.ValueUnsafe()).IfFail(() => null);
                request.Products.First().Link.Should().Be(expectedLink);
                var expectedUnits = Units.Create(requestDto.Products.First().Units).IfFail(() => null);
                request.Products.First().Units.Should().Be(expectedUnits);
                var expectedAdditionalInformation = AdditionalInformation.Create(requestDto.Products.First().AdditionalInformation).IfFail(() => null);
                request.Products.First().AdditionalInformation.IsSome.Should().BeTrue();
                request.Products.First().AdditionalInformation.IfSome(x => x.Should().Be(expectedAdditionalInformation));
                var expectedPromotionCode = PromotionCode.Create(requestDto.Products.First().PromotionCode).IfFail(() => null);
                request.Products.First().PromotionCode.IsSome.Should().BeTrue();
                request.Products.First().PromotionCode.IfSome(x => x.Should().Be(expectedPromotionCode));
                var expectedClientName = Name.Create(requestDto.Client.Email).IfFail(() => null);
                request.ClientProp.Name.Should().Be(expectedClientName);
                var expectedClientPhoneNumber = PhoneNumber.Create(requestDto.Client.PhoneNumber).IfFail(() => null);
                request.ClientProp.PhoneNumber.Should().Be(expectedClientPhoneNumber);
                var expectedClientEmail = Email.Create(requestDto.Client.Email).IfFail(() => null);
                request.ClientProp.Email.Should().Be(expectedClientEmail);
                var expectedPurchaseApplicationAdditionalInformation = AdditionalInformation.Create(requestDto.AdditionalInformation).IfFail(() => null);
                request.AdditionalInformation.IsSome.Should().BeTrue();
                request.AdditionalInformation.IfSome(x => x.Should().Be(expectedPurchaseApplicationAdditionalInformation));    
            });
        }

        [Test]
        public void DoesNotCreatePurchaseApplicationRequestWhenThereAreNotProducts()
        {
            var requestDto = BuildPurchaseApplicationCreationRequestDto(isProductListEmpty: true);

            var result = PurchaseApplicationCreationRequest.Create(requestDto);

            result.IsFail.Should().BeTrue();
            result.IfFail(validationError =>
            {
                validationError.First().FieldId.Should().Be(PluralizationProvider.Pluralize(nameof(Product)));
                validationError.First().ErrorCode.Should().Be(PurchaseApplicationCreationRequestValidationError.Required);
            });
        }
        

        private static PurchaseApplicationCreationRequestDto BuildPurchaseApplicationCreationRequestDto(
            bool isProductListEmpty = false,
            string productLink = "https://addidas.com/any/product")
        {
            return new PurchaseApplicationCreationRequestDto(
                products: BuildProducts(),
                client: new PurchaseApplicationCreationRequestDto.ClientDto(
                    name: "Alfredo",
                    phoneNumber: "123123123",
                    email: "alfredo@elguapo.com"),
                additionalInformation: "Purchase application additional information");

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