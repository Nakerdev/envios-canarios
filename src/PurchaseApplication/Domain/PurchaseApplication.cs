using System;
using System.Collections.Generic;
using System.Linq;
using CanaryDeliveries.PurchaseApplication.Domain.Entities;
using CanaryDeliveries.PurchaseApplication.Domain.ValueObjects;
using LanguageExt;

namespace CanaryDeliveries.PurchaseApplication.Domain
{
    public sealed class PurchaseApplication
    {
        public Id Id { get; }
        public IReadOnlyList<Product> Products { get; }
        public Client Client { get; }
        public Option<AdditionalInformation> AdditionalInformation { get; }
        public DateTime CreationDateTime { get; }
        public Option<DateTime> RejectionDateTime { get; }
        public Option<string> RejectionReason { get; }
        public State State => RejectionDateTime.IsSome ? State.Rejected : State.PendingOfPayment;
        
        public PersistenceStateDto PersistenceState => new PersistenceStateDto(
            id: Id.State,
            products: Products.Map(x => x.State).ToList(),
            client: Client.State,
            additionalInformation: AdditionalInformation.Map(x => x.State),
            creationDateTime: CreationDateTime);

        public PurchaseApplication(
            Id id,
            IReadOnlyList<Product> products, 
            Client client, 
            Option<AdditionalInformation> additionalInformation, 
            DateTime creationDateTime)
        {
            Id = id;
            Products = products;
            Client = client;
            AdditionalInformation = additionalInformation;
            CreationDateTime = creationDateTime;
        }
        
        public PurchaseApplication(
            Id id,
            IReadOnlyList<Product> products, 
            Client client, 
            Option<AdditionalInformation> additionalInformation, 
            DateTime creationDateTime,
            DateTime rejectionDateTime,
            string rejectionReason)
        {
            Id = id;
            Products = products;
            Client = client;
            AdditionalInformation = additionalInformation;
            CreationDateTime = creationDateTime;
            RejectionDateTime = rejectionDateTime;
            RejectionReason = rejectionReason;
        }
        
        public PurchaseApplication(PersistenceStateDto persistenceState)
        {
            Id = new Id(persistenceState.Id);
            Products = persistenceState.Products.Map(state => new Product(state)).ToList().AsReadOnly();
            Client = new Client(persistenceState.Client);
            AdditionalInformation = persistenceState.AdditionalInformation.Map(x => new AdditionalInformation(x));
            CreationDateTime = persistenceState.CreationDateTime;
        }

        public Either<Error, PurchaseApplication> Reject(DateTime rejectionDateTime, string rejectionReason)
        {
            return new PurchaseApplication(
                id: Id,
                products: Products,
                client: Client,
                additionalInformation: AdditionalInformation,
                creationDateTime: CreationDateTime,
                rejectionDateTime: rejectionDateTime,
                rejectionReason: rejectionReason);
        }
        
        public sealed class PersistenceStateDto
        {
            public Id.PersistenceState Id { get; }
            public List<Product.PersistenceState> Products { get; }
            public Client.PersistenceState Client { get; }
            public Option<AdditionalInformation.PersistenceState> AdditionalInformation { get; }
            public DateTime CreationDateTime { get; }
    
            public PersistenceStateDto(
                Id.PersistenceState id, 
                List<Product.PersistenceState> products, 
                Client.PersistenceState client, 
                Option<AdditionalInformation.PersistenceState> additionalInformation, 
                DateTime creationDateTime)
            {
                Id = id;
                Products = products;
                Client = client;
                AdditionalInformation = additionalInformation;
                CreationDateTime = creationDateTime;
            }
        }
    }

    public enum State
    {
        PendingOfPayment,
        Rejected 
    }

    public enum Error
    {
        PurchaseApplicationIsAlreadyRejected 
    }
}