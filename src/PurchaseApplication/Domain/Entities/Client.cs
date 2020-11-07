using System;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using LanguageExt;
using static LanguageExt.Prelude;

namespace CanaryDeliveries.PurchaseApplication.Domain.Entities
{
    public sealed class Client
    {
        public Name Name { get; }
        public PhoneNumber PhoneNumber { get; }
        public Email Email { get; }
        
        public static Validation<ValidationError<GenericValidationErrorCode>, Client> Create(Dto dto)
        {
            var name = Name.Create(dto.Name);
            var phoneNumber = PhoneNumber.Create(dto.PhoneNumber);
            var email = Email.Create(dto.Email);

            if (name.IsFail || phoneNumber.IsFail || email.IsFail)
            {
                var validationErrors = Seq<ValidationError<GenericValidationErrorCode>>();
                name.IfFail(errors => validationErrors = validationErrors.Concat(MapNameValidationErrors(errors)));
                phoneNumber.IfFail(errors => validationErrors = validationErrors.Concat(MapPhoneNumberValidationErrors(errors)));
                email.IfFail(errors => validationErrors = validationErrors.Concat(MapEmailValidationErrors(errors)));
                return validationErrors;
            }
                
            return new Client(
                name: name.IfFail(() => throw new InvalidOperationException()),
                phoneNumber: phoneNumber.IfFail(() => throw new InvalidOperationException()),
                email: email.IfFail(() => throw new InvalidOperationException()));

            Seq<ValidationError<GenericValidationErrorCode>> MapNameValidationErrors(
                Seq<ValidationError<GenericValidationErrorCode>> validationErrors)
            {
                return validationErrors.Map(validationError => new ValidationError<GenericValidationErrorCode>(
                    fieldId: $"{nameof(Client)}.{nameof(Name)}",
                    errorCode: validationError.ErrorCode));
            }
            
            Seq<ValidationError<GenericValidationErrorCode>> MapPhoneNumberValidationErrors(
                Seq<ValidationError<GenericValidationErrorCode>> validationErrors)
            {
                return validationErrors.Map(validationError => new ValidationError<GenericValidationErrorCode>(
                    fieldId: $"{nameof(Client)}.{nameof(PhoneNumber)}",
                    errorCode: validationError.ErrorCode));
            }
            
            Seq<ValidationError<GenericValidationErrorCode>> MapEmailValidationErrors(
                Seq<ValidationError<GenericValidationErrorCode>> validationErrors)
            {
                return validationErrors.Map(validationError => new ValidationError<GenericValidationErrorCode>(
                    fieldId: $"{nameof(Client)}.{nameof(Email)}",
                    errorCode: validationError.ErrorCode));
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
}