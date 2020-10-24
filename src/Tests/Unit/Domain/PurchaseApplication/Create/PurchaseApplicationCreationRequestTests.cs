using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.Domain.PurchaseApplication.Create;
using CanaryDeliveries.Domain.PurchaseApplication.Entities;
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

            var result = PurchaseApplicationCreationRequest.Create(requestDto);

            result .IsSuccess.Should().BeTrue();
            result .IfSuccess(request =>
            {
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
                validationError.First().FieldId.Should().Be(nameof(Product));
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

            List<Product.ProductDto> BuildProducts()
            {
                return isProductListEmpty 
                    ? new List<Product.ProductDto>() 
                    : new List<Product.ProductDto>
                    {
                        new Product.ProductDto(
                            link: productLink,
                            units: "1",
                            additionalInformation: "Product additional product",
                            promotionCode: "ADDIDAS-123")
                    };
            }
        }
    }
}