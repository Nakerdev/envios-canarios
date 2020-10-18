using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.Domain.PurchaseApplication;
using CanaryDeliveries.Domain.PurchaseApplication.Create;
using FluentAssertions;
using LanguageExt.UnsafeValueAccess;
using Moq;
using NUnit.Framework;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.Create
{
    [TestFixture]
    public sealed class CreatePurchaseApplicationTests
    {
        [Test]
        public void CreatesPurchaseApplication()
        {
            var purchaseApplicationRepository = new Mock<PurchaseApplicationRepository>();
            var command = new CreatePurchaseApplication(
                purchaseApplicationRepository: purchaseApplicationRepository.Object);
            var requestDto = new PurchaseApplicationRequestDto(
                products: new List<PurchaseApplicationRequestDto.ProductDto>
                {
                    new PurchaseApplicationRequestDto.ProductDto(
                        link: "https://addidas.com/any/product",
                        units: "1",
                        additionalInformation: "Product additional product",
                        promotionCode: "ADDIDAS-123")
                },
                client: new PurchaseApplicationRequestDto.ClientDto(
                    name: "Alfredo",
                    phoneNumber: "123123123",
                    email: "alfredo@elguapo.com"),
                additionalInformation: "Purchase application additional information");
            var purchaseApplicationCreationRequest = PurchaseApplicationCreationRequest.Create(requestDto);
            
            var createdPurchaseApplication = command.Create(purchaseApplicationCreationRequest);
            
            createdPurchaseApplication.Id.Should().NotBeNull();
            createdPurchaseApplication.Products.Count.Should().Be(requestDto.Products.Count);
            createdPurchaseApplication.Products.First().Link.Value.Should().Be(requestDto.Products.First().Link.ValueUnsafe());
            createdPurchaseApplication.Products.First().Units.Value.Should().Be(int.Parse(requestDto.Products.First().Units.ValueUnsafe()));
            createdPurchaseApplication.Products.First().AdditionalInformation.ValueUnsafe().Value.Should().Be(requestDto.Products.First().AdditionalInformation.ValueUnsafe());
            createdPurchaseApplication.Products.First().PromotionCode.ValueUnsafe().Value.Should().Be(requestDto.Products.First().PromotionCode.ValueUnsafe());
            createdPurchaseApplication.Client.Name.Value.Should().Be(requestDto.Client.Name.ValueUnsafe());
            createdPurchaseApplication.Client.PhoneNumber.Value.Should().Be(requestDto.Client.PhoneNumber.ValueUnsafe());
            createdPurchaseApplication.Client.Email.Value.Should().Be(requestDto.Client.Email.ValueUnsafe());
            createdPurchaseApplication.AdditionalInformation.ValueUnsafe().Value.Should().Be(requestDto.AdditionalInformation.ValueUnsafe());
            purchaseApplicationRepository
                .Verify(x => x.Create(It.Is<CanaryDeliveries.Domain.PurchaseApplication.PurchaseApplication>(y => 
                    y.Id != null
                    && y.Products.Count == requestDto.Products.Count
                    && y.Products.First().Link.Value == requestDto.Products.First().Link.ValueUnsafe()
                    && y.Products.First().Units.Value == int.Parse(requestDto.Products.First().Units.ValueUnsafe())
                    && y.Products.First().AdditionalInformation.ValueUnsafe().Value == requestDto.Products.First().AdditionalInformation.ValueUnsafe()
                    && y.Products.First().PromotionCode.ValueUnsafe().Value == requestDto.Products.First().PromotionCode.ValueUnsafe()
                    && y.Client.Name.Value == requestDto.Client.Name.ValueUnsafe()
                    && y.Client.PhoneNumber.Value == requestDto.Client.PhoneNumber.ValueUnsafe()
                    && y.Client.Email.Value == requestDto.Client.Email.ValueUnsafe()
                    && y.AdditionalInformation.ValueUnsafe().Value == requestDto.AdditionalInformation.ValueUnsafe())), Times.Once);
        }
    }
}