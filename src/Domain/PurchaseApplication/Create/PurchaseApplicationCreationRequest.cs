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
            return products
                .Map(p => new PurchaseApplicationCreationRequest(
                    products: p,
                    clientProp: BuildClient(),
                    additionalInformation: creationRequestDto.AdditionalInformation.Map(
                        value => Domain.PurchaseApplication.ValueObjects.AdditionalInformation.Create(value).IfFail(() => throw new InvalidOperationException()))))
                .MapFail(error => new ValidationError<PurchaseApplicationCreationRequestValidationError>(
                        fieldId: error.FieldId,
                        errorCode: PurchaseApplicationCreationRequestValidationError.Required));

            Client BuildClient()
            {
                return new Client(
                    name: Name.Create(creationRequestDto.Client.Name).ValueUnsafe(),
                    phoneNumber: PhoneNumber.Create(creationRequestDto.Client.PhoneNumber).ValueUnsafe(),
                    email: Email.Create(creationRequestDto.Client.Email).ValueUnsafe());
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

        public sealed class Client
        {
            public Name Name { get; }
            public PhoneNumber PhoneNumber { get; }
            public Email Email { get; }
    
            public Client(
                Name name, 
                PhoneNumber phoneNumber, 
                Email email)
            {
                Name = name;
                PhoneNumber = phoneNumber;
                Email = email;
            }
        }
    }
    
    public sealed class PurchaseApplicationCreationRequestDto
    {
        public List<Product.ProductDto> Products { get; }
        public ClientDto Client { get; }
        public Option<string> AdditionalInformation { get; }

        public PurchaseApplicationCreationRequestDto(
            List<Product.ProductDto> products, 
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
        Required
    }
}