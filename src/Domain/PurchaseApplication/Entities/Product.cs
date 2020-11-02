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
            ValidationError<ProductValidationErrorCode>, 
            IReadOnlyList<Product>> Create(IReadOnlyList<Dto> productsDto)
        {
            if (productsDto.Count == 0)
            {
                return new ValidationError<ProductValidationErrorCode>(
                    fieldId: PluralizationProvider.Pluralize(nameof(Product)),
                    errorCode: ProductValidationErrorCode.Required);
            }
           
            var validationErrors = new Seq<ValidationError<ProductValidationErrorCode>>();
            var products = new List<Product>();
            productsDto
                .Select((productDto, index) => new {Value = productDto, Index = index} )
                .ToList()
                .ForEach(productDto =>
                {
                   var link = Link.Create(productDto.Value.Link);
  
                   if (link.IsFail)
                   {
                       link.IfFail(errors => validationErrors = validationErrors.Concat(MapLinkValidationErrors(errors, productDto.Index)));
                   }
                   else
                   {
                       var product = new Product(
                           link: link.ToEither().ValueUnsafe(),
                           units: Units.Create(productDto.Value.Units)
                               .IfFail(() => throw new InvalidOperationException()),
                           productDto.Value.AdditionalInformation.Map(
                               x => Domain.PurchaseApplication.ValueObjects.AdditionalInformation.Create(x)
                                   .IfFail(() => throw new InvalidOperationException())),
                           promotionCode: productDto.Value.PromotionCode.Map(x =>
                               Domain.PurchaseApplication.ValueObjects.PromotionCode.Create(x)
                                   .IfFail(() => throw new InvalidOperationException()))); 
                          products.Add(product);
                   }                  
                });

            if (validationErrors.Count > 0)
            {
                return validationErrors;
            }
            return products.ToList().AsReadOnly();
            
            Seq<ValidationError<ProductValidationErrorCode>> MapLinkValidationErrors(
                Seq<ValidationError<LinkValidationErrorCode>> validationErrors,
                int index)
            {
                return validationErrors.Map(validationError => new ValidationError<ProductValidationErrorCode>(
                    fieldId: $"{nameof(Product)}[{index}].{nameof(Link)}",
                    errorCode: MapErrorCode(validationError.ErrorCode)));

                static ProductValidationErrorCode MapErrorCode(LinkValidationErrorCode errorCode)
                {
                    var errorsEquality = new Dictionary<LinkValidationErrorCode, ProductValidationErrorCode>
                    {
                        {LinkValidationErrorCode.Required, ProductValidationErrorCode.Required},
                        {LinkValidationErrorCode.WrongLength, ProductValidationErrorCode.WrongLength},
                        {LinkValidationErrorCode.InvalidFormat, ProductValidationErrorCode.InvalidFormat}
                    };
                    return errorsEquality[errorCode];
                }
            }
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

    public enum ProductValidationErrorCode
    {
        Required,
        WrongLength,
        InvalidFormat
    }
}