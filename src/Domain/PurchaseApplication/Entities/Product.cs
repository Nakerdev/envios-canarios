using System;
using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.Domain.PurchaseApplication.Create;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using PluralizeService.Core;

namespace CanaryDeliveries.Domain.PurchaseApplication.Entities
{
    public sealed class Product
    {
        public Link Link { get; }
        public Units Units { get; }
        public Option<AdditionalInformation> AdditionalInformation { get; }
        public Option<PromotionCode> PromotionCode { get; }

        public static Validation<
            ValidationError<ProductValidationError>, 
            IReadOnlyList<Product>> Create(IReadOnlyList<Dto> productsDto)
        {
            if (productsDto.Count == 0)
            {
                return new ValidationError<ProductValidationError>(
                    fieldId: PluralizationProvider.Pluralize(nameof(Product)),
                    errorCode: ProductValidationError.Required);
            }

            return productsDto
                .Map(product => new Product(
                    link: Link.Create(product.Link).IfFail(() => throw new InvalidOperationException()),
                    units: Units.Create(product.Units).IfFail(() => throw new InvalidOperationException()),
                    product.AdditionalInformation.Map(
                        x => Domain.PurchaseApplication.ValueObjects.AdditionalInformation.Create(x).IfFail(() => throw new InvalidOperationException())),
                    promotionCode: product.PromotionCode.Map(x => 
                        Domain.PurchaseApplication.ValueObjects.PromotionCode.Create(x).IfFail(() => throw new InvalidOperationException()))))
                .ToList()
                .AsReadOnly();
        }

        public Product(
            Link link, 
            Units units, 
            Option<AdditionalInformation> additionalInformation, 
            Option<PromotionCode> promotionCode)
        {
            Link = link;
            Units = units;
            AdditionalInformation = additionalInformation;
            PromotionCode = promotionCode;
        }
        
        public class Dto
        {
            public Option<string> Link { get; }
            public Option<string> Units { get; }
            public Option<string> AdditionalInformation { get; }
            public Option<string> PromotionCode { get; }

            public Dto(
                Option<string> link, 
                Option<string> units, 
                Option<string> additionalInformation, 
                Option<string> promotionCode)
            {
                Link = link;
                Units = units;
                AdditionalInformation = additionalInformation;
                PromotionCode = promotionCode;
            } 
        }
    }

    public enum ProductValidationError
    {
        Required
    }
}