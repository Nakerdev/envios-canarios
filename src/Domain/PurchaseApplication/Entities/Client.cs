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
            var name = Name.Create(dto.Name);
            var phoneNumber = PhoneNumber.Create(dto.PhoneNumber);
            var email = Email.Create(dto.Email);

            if (name.IsFail || phoneNumber.IsFail || email.IsFail)
            {
                var validationErrors = Seq<ValidationError<ClientValidationErrorCode>>();
                name.IfFail(errors => validationErrors = validationErrors.Concat(MapNameValidationErrors(errors)));
                phoneNumber.IfFail(errors => validationErrors = validationErrors.Concat(MapPhoneNumberValidationErrors(errors)));
                email.IfFail(errors => validationErrors = validationErrors.Concat(MapEmailValidationErrors(errors)));
                return validationErrors;
            }
                
            return new Client(
                name: name.ToEither().ValueUnsafe(),
                phoneNumber: phoneNumber.ToEither().ValueUnsafe(),
                email: email.ToEither().ValueUnsafe());

            Seq<ValidationError<ClientValidationErrorCode>> MapNameValidationErrors(
                Seq<ValidationError<NameValidationErrorCode>> validationErrors)
            {
                return validationErrors.Map(validationError => new ValidationError<ClientValidationErrorCode>(
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
            
            Seq<ValidationError<ClientValidationErrorCode>> MapPhoneNumberValidationErrors(
                Seq<ValidationError<PhoneNumberValidationErrorCode>> validationErrors)
            {
                return validationErrors.Map(validationError => new ValidationError<ClientValidationErrorCode>(
                    fieldId: $"{nameof(Client)}.{nameof(PhoneNumber)}",
                    errorCode: MapErrorCode(validationError.ErrorCode)));

                static ClientValidationErrorCode MapErrorCode(PhoneNumberValidationErrorCode errorCode)
                {
                    var errorsEquality = new Dictionary<PhoneNumberValidationErrorCode, ClientValidationErrorCode>
                    {
                        {PhoneNumberValidationErrorCode.Required, ClientValidationErrorCode.Required},
                        {PhoneNumberValidationErrorCode.InvalidFormat, ClientValidationErrorCode.InvalidFormat},
                        {PhoneNumberValidationErrorCode.WrongLenght, ClientValidationErrorCode.WrongLength}
                    };
                    return errorsEquality[errorCode];
                }
            }
            
            Seq<ValidationError<ClientValidationErrorCode>> MapEmailValidationErrors(
                Seq<ValidationError<EmailValidationErrorCode>> validationErrors)
            {
                return validationErrors.Map(validationError => new ValidationError<ClientValidationErrorCode>(
                    fieldId: $"{nameof(Client)}.{nameof(Email)}",
                    errorCode: MapErrorCode(validationError.ErrorCode)));

                static ClientValidationErrorCode MapErrorCode(EmailValidationErrorCode errorCode)
                {
                    var errorsEquality = new Dictionary<EmailValidationErrorCode, ClientValidationErrorCode>
                    {
                        {EmailValidationErrorCode.Required, ClientValidationErrorCode.Required},
                        {EmailValidationErrorCode.InvalidFormat, ClientValidationErrorCode.InvalidFormat},
                        {EmailValidationErrorCode.WrongLength, ClientValidationErrorCode.WrongLength}
                    };
                    return errorsEquality[errorCode];
                }
            }
        }

        private Client(
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
        WrongLength,
        InvalidFormat
    }
}