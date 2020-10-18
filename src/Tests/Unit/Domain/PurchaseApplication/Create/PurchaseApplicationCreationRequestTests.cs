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
            request.Products.First().Link.Should().Be(new Link(requestDto.Products.First().Link.ValueUnsafe()));
            request.Products.First().Units.Should().Be(new Units(int.Parse(requestDto.Products.First().Units.ValueUnsafe())));
            //Me quedo aqui, pendiente de resolver dudas respecto a como validar los Value Objects.
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