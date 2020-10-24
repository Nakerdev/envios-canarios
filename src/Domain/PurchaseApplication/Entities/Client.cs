using CanaryDeliveries.Domain.PurchaseApplication.ValueObjects;

namespace CanaryDeliveries.Domain.PurchaseApplication.Entities
{
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