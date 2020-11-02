using CanaryDeliveries.Domain.PurchaseApplication.Create;
using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;
using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.Entities
{
    public sealed class Client
    {
        public Name Name { get; }
        public PhoneNumber PhoneNumber { get; }
        public Email Email { get; }
        
        public static Validation<ValidationError<EmailValidationErrorCode>, Client> Create(ClientDto clientDto)
        {
            return new Client(
                name: Name.Create(clientDto.Email).IfFail(() => null), 
                phoneNumber: PhoneNumber.Create(clientDto.PhoneNumber).IfFail(() => null),
                email: Email.Create(clientDto.Email).IfFail(() => null));
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

    public enum ClientValidationErrorCode
    {
        Required
    }
}