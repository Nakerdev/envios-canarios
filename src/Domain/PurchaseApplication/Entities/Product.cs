using System;
using System.Collections.Generic;
using System.Linq;
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
            ValidationError<GenericValidationErrorCode>, 
            IReadOnlyList<Product>> Create(IReadOnlyList<Dto> productsDto)
        {
            if (productsDto.Count == 0)
            {
                return new ValidationError<GenericValidationErrorCode>(
                    fieldId: PluralizationProvider.Pluralize(nameof(Product)),
                    errorCode: GenericValidationErrorCode.Required);
            }
           
            var validationErrors = new Seq<ValidationError<GenericValidationErrorCode>>();
            var products = new List<Product>();
            productsDto
                .Select((productDto, index) => new {Value = productDto, Index = index} )
                .ToList()
                .ForEach(productDto =>
                {
                   var link = Link.Create(productDto.Value.Link);
                   var units = Units.Create(productDto.Value.Units);
                   var additionalInformation = productDto.Value.AdditionalInformation
                       .Map(x => Domain.PurchaseApplication.ValueObjects.AdditionalInformation.Create(x));
                   var promotionCode = productDto.Value.PromotionCode
                        .Map(x => Domain.PurchaseApplication.ValueObjects.PromotionCode.Create(x));
  
                   if (link.IsFail 
                       || units.IsFail 
                       || additionalInformation.Match(None: () => false, Some: x => x.IsFail)
                       || promotionCode.Match(None: () => false, Some: x => x.IsFail))
                   {
                       link.IfFail(errors => validationErrors = validationErrors.Concat(MapLinkValidationErrors(errors, productDto.Index)));
                       units.IfFail(errors => validationErrors = validationErrors.Concat(MapUnitsValidationErrors(errors, productDto.Index)));
                       additionalInformation.IfSome(result =>
                           result.IfFail(errors => validationErrors = validationErrors.Concat(MapAdditionalInformationValidationErrors(errors, productDto.Index))));
                       promotionCode.IfSome(result =>
                            result.IfFail(errors => validationErrors = validationErrors.Concat(MapPromotionCodeValidationErrors(errors, productDto.Index))));
                   }
                   else
                   {
                       var product = new Product(
                           link: link.IfFail(() => throw new InvalidOperationException()),
                           units: units.IfFail(() => throw new InvalidOperationException()),
                           additionalInformation: additionalInformation.Match(
                               None: () => null, 
                               Some: x => x.IfFail(() => throw new InvalidOperationException())),
                           promotionCode: promotionCode.Match(
                               None: () => null, 
                               Some: x => x.IfFail(() => throw new InvalidOperationException())));
                       products.Add(product);
                   }                  
                });

            if (validationErrors.Count > 0)
            {
                return validationErrors;
            }
            return products.ToList().AsReadOnly();
            
            Seq<ValidationError<GenericValidationErrorCode>> MapLinkValidationErrors(
                Seq<ValidationError<GenericValidationErrorCode>> validationErrors,
                int index)
            {
                return validationErrors.Map(validationError => new ValidationError<GenericValidationErrorCode>(
                    fieldId: $"{nameof(Product)}[{index}].{nameof(Link)}",
                    errorCode: validationError.ErrorCode));
            }
            
            Seq<ValidationError<GenericValidationErrorCode>> MapUnitsValidationErrors(
                Seq<ValidationError<GenericValidationErrorCode>> validationErrors,
                int index)
            {
                return validationErrors.Map(validationError => new ValidationError<GenericValidationErrorCode>(
                    fieldId: $"{nameof(Product)}[{index}].{nameof(Units)}",
                    errorCode: validationError.ErrorCode));
            }
            
            Seq<ValidationError<GenericValidationErrorCode>> MapAdditionalInformationValidationErrors(
                Seq<ValidationError<GenericValidationErrorCode>> validationErrors,
                int index)
            {
                return validationErrors.Map(validationError => new ValidationError<GenericValidationErrorCode>(
                    fieldId: $"{nameof(Product)}[{index}].{nameof(AdditionalInformation)}",
                    errorCode: validationError.ErrorCode));
            }
            
            Seq<ValidationError<GenericValidationErrorCode>> MapPromotionCodeValidationErrors(
                Seq<ValidationError<GenericValidationErrorCode>> validationErrors,
                int index)
            {
                return validationErrors.Map(validationError => new ValidationError<GenericValidationErrorCode>(
                    fieldId: $"{nameof(Product)}[{index}].{nameof(PromotionCode)}",
                    errorCode: validationError.ErrorCode));
            }
        }

        private Product(
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
}