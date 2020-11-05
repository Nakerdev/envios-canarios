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
            
            if (products.IsFail)
            {
                var validationErrors = Prelude.Seq<ValidationError<PurchaseApplicationCreationRequestValidationError>>();
                products.IfFail(errors => validationErrors = validationErrors.Concat(MapProductValidationErrors(errors)));
                return validationErrors;
            }

            return new PurchaseApplicationCreationRequest(
                products: products.ToEither().ValueUnsafe(),
                clientProp: BuildClient(),
                additionalInformation: creationRequestDto.AdditionalInformation.Map(
                    value => Domain.PurchaseApplication.ValueObjects.AdditionalInformation.Create(value)
                        .IfFail(() => throw new InvalidOperationException())));

            Client BuildClient()
            {
                var dto = new Client.Dto(                    
                    name: creationRequestDto.Client.Name,
                    phoneNumber: creationRequestDto.Client.PhoneNumber,
                    email: creationRequestDto.Client.Email);
                return Client.Create(dto).IfFail(() => throw new InvalidOperationException());
            }
            
            Seq<ValidationError<PurchaseApplicationCreationRequestValidationError>> MapProductValidationErrors(
                Seq<ValidationError<ProductValidationErrorCode>> validationErrors)
            {
                return validationErrors.Map(validationError => new ValidationError<PurchaseApplicationCreationRequestValidationError>(
                    fieldId: $"{nameof(Client)}.{nameof(Name)}",
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
        public ClientDto Client { get; }
        public Option<string> AdditionalInformation { get; }

        public PurchaseApplicationCreationRequestDto(
            List<Product.Dto> products, 
            ClientDto client, 
            Option<string> additionalInformation)
        {
            Products = products;
            Client = client;
            AdditionalInformation = additionalInformation;
        }
        
        public sealed class ClientDto
        {
            public Option<string> Name { get; }
            public Option<string> PhoneNumber { get; }
            public Option<string> Email { get; }

            public ClientDto(
                Option<string> name, 
                Option<string> phoneNumber, 
                Option<string> email)
            {
                Name = name;
                PhoneNumber = phoneNumber;
                Email = email;
            }
        }
    }

    public sealed class ValidationError<T>
    {
        public string FieldId { get; }
        public T ErrorCode { get; }

        public ValidationError(string fieldId, T errorCode)
        {
            FieldId = fieldId;
            ErrorCode = errorCode;
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