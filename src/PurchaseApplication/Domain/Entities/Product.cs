using System;
using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using LanguageExt;
using PluralizeService.Core;

namespace CanaryDeliveries.PurchaseApplication.Domain.Entities
{
    public sealed class Product
    {
        public Id Id { get; }
        public Link Link { get; }
        public Units Units { get; }
        public Option<AdditionalInformation> AdditionalInformation { get; }
        public Option<PromotionCode> PromotionCode { get; }
        public PersistenceState State => new PersistenceState(
            Id.State,
            Link.State,
            Units.State,
            AdditionalInformation.Map(x => x.State),
            PromotionCode.Map(x => x.State));


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
                       .Map(x => ValueObjects.AdditionalInformation.Create(x));
                   var promotionCode = productDto.Value.PromotionCode
                        .Map(x => ValueObjects.PromotionCode.Create(x));
  
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
                           id: Id.Create(),
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

        public Product(PersistenceState persistenceState)
        {
            Id = new Id(persistenceState.Id);
            Link = new Link(persistenceState.Link);
            Units = new Units(persistenceState.Units);
            AdditionalInformation = persistenceState.AdditionalInformation.Map(x => new AdditionalInformation(x));
            PromotionCode = persistenceState.PromotionCode.Map(x => new PromotionCode(x));
        }

        private Product(
            Id id,
            Link link, 
            Units units, 
            Option<AdditionalInformation> additionalInformation, 
            Option<PromotionCode> promotionCode)
        {
            Id = id;
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

        public sealed class PersistenceState
        {
            public Id.PersistenceState Id { get; }
            public Link.PersistenceState Link { get; }
            public Units.PersistenceState Units { get; }
            public Option<AdditionalInformation.PersistenceState> AdditionalInformation { get; }
            public Option<PromotionCode.PersistenceState> PromotionCode { get; }

            public PersistenceState(
                Id.PersistenceState id,
                Link.PersistenceState link, 
                Units.PersistenceState units, 
                Option<AdditionalInformation.PersistenceState> additionalInformation, 
                Option<PromotionCode.PersistenceState> promotionCode)
            {
                Id = id;
                Link = link;
                Units = units;
                AdditionalInformation = additionalInformation;
                PromotionCode = promotionCode;
            }
        }
    }
}