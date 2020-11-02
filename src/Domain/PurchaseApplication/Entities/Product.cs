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
                       products.Add(new Product(
                          link: link.ToEither().ValueUnsafe(),
                          units: units.ToEither().ValueUnsafe(),
                          additionalInformation: additionalInformation.Match(None: () => null, Some: x => x.ToEither().ValueUnsafe()),
                          promotionCode: promotionCode.Match(None: () => null, Some: x => x.ToEither().ValueUnsafe())));
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
            
            Seq<ValidationError<ProductValidationErrorCode>> MapUnitsValidationErrors(
                Seq<ValidationError<UnitsValidationErrorCode>> validationErrors,
                int index)
            {
                return validationErrors.Map(validationError => new ValidationError<ProductValidationErrorCode>(
                    fieldId: $"{nameof(Product)}[{index}].{nameof(Units)}",
                    errorCode: MapErrorCode(validationError.ErrorCode)));

                static ProductValidationErrorCode MapErrorCode(UnitsValidationErrorCode errorCode)
                {
                    var errorsEquality = new Dictionary<UnitsValidationErrorCode, ProductValidationErrorCode>
                    {
                        {UnitsValidationErrorCode.Required, ProductValidationErrorCode.Required},
                        {UnitsValidationErrorCode.InvalidValue, ProductValidationErrorCode.InvalidValue},
                        {UnitsValidationErrorCode.InvalidFormat, ProductValidationErrorCode.InvalidFormat}
                    };
                    return errorsEquality[errorCode];
                }
            }
            
            Seq<ValidationError<ProductValidationErrorCode>> MapAdditionalInformationValidationErrors(
                Seq<ValidationError<AdditionalInformationValidationErrorCode>> validationErrors,
                int index)
            {
                return validationErrors.Map(validationError => new ValidationError<ProductValidationErrorCode>(
                    fieldId: $"{nameof(Product)}[{index}].{nameof(AdditionalInformation)}",
                    errorCode: MapErrorCode(validationError.ErrorCode)));

                static ProductValidationErrorCode MapErrorCode(AdditionalInformationValidationErrorCode errorCode)
                {
                    var errorsEquality = new Dictionary<AdditionalInformationValidationErrorCode, ProductValidationErrorCode>
                    {
                        {AdditionalInformationValidationErrorCode.Required, ProductValidationErrorCode.Required},
                        {AdditionalInformationValidationErrorCode.WrongLength, ProductValidationErrorCode.WrongLength}
                    };
                    return errorsEquality[errorCode];
                }
            }
            
            Seq<ValidationError<ProductValidationErrorCode>> MapPromotionCodeValidationErrors(
                Seq<ValidationError<PromotionCodeValidationErrorCode>> validationErrors,
                int index)
            {
                return validationErrors.Map(validationError => new ValidationError<ProductValidationErrorCode>(
                    fieldId: $"{nameof(Product)}[{index}].{nameof(PromotionCode)}",
                    errorCode: MapErrorCode(validationError.ErrorCode)));

                static ProductValidationErrorCode MapErrorCode(PromotionCodeValidationErrorCode errorCode)
                {
                    var errorsEquality = new Dictionary<PromotionCodeValidationErrorCode, ProductValidationErrorCode>
                    {
                        {PromotionCodeValidationErrorCode.Required, ProductValidationErrorCode.Required},
                        {PromotionCodeValidationErrorCode.WrongLength, ProductValidationErrorCode.WrongLength}
                    };
                    return errorsEquality[errorCode];
                }
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

    public enum ProductValidationErrorCode
    {
        Required,
        WrongLength,
        InvalidFormat,
        InvalidValue
    }
}