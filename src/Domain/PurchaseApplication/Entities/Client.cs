using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CanaryDeliveries.Domain.PurchaseApplication.Create;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.Domain.PurchaseApplication.Entities
{
    public sealed class Client
    {
        public Name Name { get; }
        public PhoneNumber PhoneNumber { get; }
        public Email Email { get; }
        
        public static Validation<ValidationError<ClientValidationErrorCode>, Client> Create(Dto dto)
        {
            var name = CreateName(dto.Name);

            if (!name.IsSuccess)
            {
                var validationErrors = Seq<ValidationError<ClientValidationErrorCode>>();
                name.IfFail(errors => validationErrors = validationErrors.Concat(errors));
                return validationErrors;
            }
                
            return new Client(
                name: name.ToEither().ValueUnsafe(),
                phoneNumber: PhoneNumber.Create(dto.PhoneNumber).IfFail(() => null),
                email: Email.Create(dto.Email).IfFail(() => null));

            Validation<ValidationError<ClientValidationErrorCode>, Name> CreateName(Option<string> name)
            {
                return Name
                    .Create(name)
                    .MapFail(validationError => new ValidationError<ClientValidationErrorCode>(
                        fieldId: $"{nameof(Client)}.{nameof(Name)}",
                        errorCode: MapErrorCode(validationError.ErrorCode)));

                static ClientValidationErrorCode MapErrorCode(NameValidationErrorCode errorCode)
                {
                    var errorsEquality = new Dictionary<NameValidationErrorCode, ClientValidationErrorCode>
                    {
                        {NameValidationErrorCode.Required, ClientValidationErrorCode.Required},
                        {NameValidationErrorCode.WrongLength, ClientValidationErrorCode.WrongLength}
                    };
                    return errorsEquality[errorCode];
                }
            }
        }

        public Client(
            Name name, 
            PhoneNumber phoneNumber, 
            Email email)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
        }
        
        public sealed class Dto
        {
            public Option<string> Name { get; }
            public Option<string> PhoneNumber { get; }
            public Option<string> Email { get; }

            public Dto(
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

    public enum ClientValidationErrorCode
    {
        Required,
        WrongLength
    }
}