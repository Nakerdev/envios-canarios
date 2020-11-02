using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.Domain.PurchaseApplication.Entities;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using FluentAssertions;
using NUnit.Framework;
using PluralizeService.Core;

namespace CanaryDeliveries.Tests.Domain.PurchaseApplication.Entities
{
    public class ProductTests
    {
        [Test]
        public void CreatesProductList()
        {
            var productDto = buildProductDto();
            
            var result = Product.Create(new List<Product.Dto>{productDto}.AsReadOnly());

            result.IsSuccess.Should().BeTrue();
            result.IfSuccess(products =>
            {
                products.Count.Should().Be(1);
                var expectedLink = Link.Create(productDto.Link).IfFail(() => null);
                products.First().Link.Should().Be(expectedLink);
                var expectedUnits = Units.Create(productDto.Units).IfFail(() => null);
                products.First().Units.Should().Be(expectedUnits);
                products.First().AdditionalInformation.IsSome.Should().BeTrue();
                var expectedAdditionalInformation = AdditionalInformation.Create(productDto.AdditionalInformation).IfFail(() => null);
                products.First().AdditionalInformation.IfSome(x => x.Should().Be(expectedAdditionalInformation));
                var expectedPromotionCode = PromotionCode.Create(productDto.PromotionCode).IfFail(() => null);
                products.First().PromotionCode.IsSome.Should().BeTrue();
                products.First().PromotionCode.IfSome(x => x.Should().Be(expectedPromotionCode));
            });
        }
        
        [Test]
        public void DoesNotCreateProductsWhenThereAreNotProducts()
        {
            var result = Product.Create(new List<Product.Dto>().AsReadOnly());

            result.IsFail.Should().BeTrue();
            result.IfFail(validationError =>
            {
                validationError.First().FieldId.Should().Be(PluralizationProvider.Pluralize(nameof(Product)));
                validationError.First().ErrorCode.Should().Be(ProductValidationErrorCode.Required);
            });
        }
        
        [Test]
        public void DoesNotCreateProductsWhenSomeLinkHasValidationErrors()
        {
            var product = buildProductDto(link: null);
            
            var result = Product.Create(new List<Product.Dto>{product}.AsReadOnly());

            result.IsFail.Should().BeTrue();
            result.IfFail(validationErrors =>
            {
                validationErrors.Count.Should().Be(1);
                validationErrors.First().FieldId.Should().Be($"{nameof(Product)}[0].{nameof(Link)}");
            });
        }
        
        [Test]
        public void DoesNotCreateProductsWhenSomeUnitsHasValidationErrors()
        {
            var product = buildProductDto(units: null);
            
            var result = Product.Create(new List<Product.Dto>{product}.AsReadOnly());

            result.IsFail.Should().BeTrue();
            result.IfFail(validationErrors =>
            {
                validationErrors.Count.Should().Be(1);
                validationErrors.First().FieldId.Should().Be($"{nameof(Product)}[0].{nameof(Units)}");
            });
        }
        
        [Test]
        public void DoesNotCreateProductsWhenSomeAdditionalInformationHasValidationErrors()
        {
            var product = buildProductDto(additionalInformation: new string('a', 1001));
            
            var result = Product.Create(new List<Product.Dto>{product}.AsReadOnly());

            result.IsFail.Should().BeTrue();
            result.IfFail(validationErrors =>
            {
                validationErrors.Count.Should().Be(1);
                validationErrors.First().FieldId.Should().Be($"{nameof(Product)}[0].{nameof(AdditionalInformation)}");
            });
        }
        
        private static Product.Dto buildProductDto(
            string link = "https://addidas.com/products/1", 
            string units = "1", 
            string additionalInformation = "Additional information", 
            string promotionCode = "ADDIDAS-123")
        {
            return new Product.Dto(
                link: link,
                units: units,
                additionalInformation: additionalInformation,
                promotionCode: promotionCode);
        }
    }
}