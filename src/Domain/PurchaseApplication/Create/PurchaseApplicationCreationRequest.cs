using System;
using System.Collections.Generic;
using CanaryDeliveries.Domain.PurchaseApplication.Entities;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace CanaryDeliveries.Domain.PurchaseApplication.Create
{
    public sealed class PurchaseApplicationCreationRequest
    {
        public IReadOnlyList<Product> Products { get; }
        public Client ClientProp { get; }
        public Option<AdditionalInformation> AdditionalInformation { get; }

        public static Validation<
            ValidationError<PurchaseApplicationCreationRequestValidationError>, 
            PurchaseApplicationCreationRequest> Create(PurchaseApplicationCreationRequestDto creationRequestDto)
        {
            var products = Product.Create(creationRequestDto.Products);
            var client = Client.Create(creationRequestDto.Client);
            var additionalInformation = creationRequestDto.AdditionalInformation.Map(x => Domain.PurchaseApplication.ValueObjects.AdditionalInformation.Create(x));
            
            if (products.IsFail 
                || client.IsFail 
                || additionalInformation.Match(None: () => false, Some: x => x.IsFail)
)
            {
                var validationErrors = Prelude.Seq<ValidationError<PurchaseApplicationCreationRequestValidationError>>();
                products.IfFail(errors => validationErrors = validationErrors.Concat(MapProductValidationErrors(errors)));
                client.IfFail(errors => validationErrors = validationErrors.Concat(MapClientValidationErrors(errors)));
                additionalInformation.IfSome(result =>
                    result.IfFail(errors => validationErrors = validationErrors.Concat(MapAdditionalInformationValidationErrors(errors))));
                return validationErrors;
            }

            return new PurchaseApplicationCreationRequest(
                products: products.ToEither().ValueUnsafe(),
                clientProp: client.ToEither().ValueUnsafe(),
                additionalInformation: additionalInformation.Match(None: () => null, Some: x => x.ToEither().ValueUnsafe()));
            
            Seq<ValidationError<PurchaseApplicationCreationRequestValidationError>> MapProductValidationErrors(
                Seq<ValidationError<ProductValidationErrorCode>> validationErrors)
            {
                return validationErrors.Map(validationError => new ValidationError<PurchaseApplicationCreationRequestValidationError>(
                    fieldId: validationError.FieldId,
                    errorCode: MapErrorCode(validationError.ErrorCode)));

                static PurchaseApplicationCreationRequestValidationError MapErrorCode(ProductValidationErrorCode errorCode)
                {
                    var errorsEquality = new Dictionary<ProductValidationErrorCode, PurchaseApplicationCreationRequestValidationError >
                    {
                        {ProductValidationErrorCode.Required, PurchaseApplicationCreationRequestValidationError.Required},
                        {ProductValidationErrorCode.InvalidFormat, PurchaseApplicationCreationRequestValidationError.InvalidFormat},
                        {ProductValidationErrorCode.InvalidValue, PurchaseApplicationCreationRequestValidationError.InvalidValue},
                        {ProductValidationErrorCode.WrongLength, PurchaseApplicationCreationRequestValidationError.WrongLength}
                    };
                    return errorsEquality[errorCode];
                }
            }
            
            Seq<ValidationError<PurchaseApplicationCreationRequestValidationError>> MapClientValidationErrors(
                Seq<ValidationError<ClientValidationErrorCode>> validationErrors)
            {
                return validationErrors.Map(validationError => new ValidationError<PurchaseApplicationCreationRequestValidationError>(
                    fieldId: validationError.FieldId,
                    errorCode: MapErrorCode(validationError.ErrorCode)));

                static PurchaseApplicationCreationRequestValidationError MapErrorCode(ClientValidationErrorCode errorCode)
                {
                    var errorsEquality = new Dictionary<ClientValidationErrorCode, PurchaseApplicationCreationRequestValidationError >
                    {
                        {ClientValidationErrorCode.Required, PurchaseApplicationCreationRequestValidationError.Required},
                        {ClientValidationErrorCode.InvalidFormat, PurchaseApplicationCreationRequestValidationError.InvalidFormat},
                        {ClientValidationErrorCode.WrongLength, PurchaseApplicationCreationRequestValidationError.WrongLength}
                    };
                    return errorsEquality[errorCode];
                }
            }
            
            Seq<ValidationError<PurchaseApplicationCreationRequestValidationError>> MapAdditionalInformationValidationErrors(
                Seq<ValidationError<AdditionalInformationValidationErrorCode>> validationErrors)
            {
                return validationErrors.Map(validationError => new ValidationError<PurchaseApplicationCreationRequestValidationError>(
                    fieldId: validationError.FieldId,
                    errorCode: MapErrorCode(validationError.ErrorCode)));

                static PurchaseApplicationCreationRequestValidationError MapErrorCode(AdditionalInformationValidationErrorCode errorCode)
                {
                    var errorsEquality = new Dictionary<AdditionalInformationValidationErrorCode, PurchaseApplicationCreationRequestValidationError >
                    {
                        {AdditionalInformationValidationErrorCode.Required, PurchaseApplicationCreationRequestValidationError.Required},
                        {AdditionalInformationValidationErrorCode.WrongLength, PurchaseApplicationCreationRequestValidationError.WrongLength}
                    };
                    return errorsEquality[errorCode];
                }
            }
        }

        private PurchaseApplicationCreationRequest(
            IReadOnlyList<Product> products, 
            Client clientProp, 
            Option<AdditionalInformation> additionalInformation)
        {
            Products = products;
            ClientProp = clientProp;
            AdditionalInformation = additionalInformation;
        }
    }
    
    public sealed class PurchaseApplicationCreationRequestDto
    {
        public List<Product.Dto> Products { get; }
        public Client.Dto Client { get; }
        public Option<string> AdditionalInformation { get; }

        public PurchaseApplicationCreationRequestDto(
            List<Product.Dto> products, 
            Client.Dto client, 
            Option<string> additionalInformation)
        {
            Products = products;
            Client = client;
            AdditionalInformation = additionalInformation;
        }
    }

    public enum PurchaseApplicationCreationRequestValidationError
    {
        Required,
        InvalidFormat,
        InvalidValue,
        WrongLength
    }
}