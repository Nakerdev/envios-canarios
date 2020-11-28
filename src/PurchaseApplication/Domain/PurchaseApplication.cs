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
        public PersistenceState State => new PersistenceState(
            id: Id.State,
            products: Products.Map(x => x.State).ToList(),
            client: Client.State,
            additionalInformation: AdditionalInformation.Map(x => x.State),
            creationDate: CreationDateTime);

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
        
        public PurchaseApplication(PersistenceState persistenceState)
        {
            Id = new Id(persistenceState.Id);
            Products = persistenceState.Products.Map(state => new Product(state)).ToList().AsReadOnly();
            Client = new Client(persistenceState.Client);
            AdditionalInformation = persistenceState.AdditionalInformation.Map(x => new AdditionalInformation(x));
            CreationDateTime = persistenceState.CreationDate;
        }
        
        public sealed class PersistenceState
        {
            public Id.PersistenceState Id { get; }
            public List<Product.PersistenceState> Products { get; }
            public Client.PersistenceState Client { get; }
            public Option<AdditionalInformation.PersistenceState> AdditionalInformation { get; }
            public DateTime CreationDate { get; }
    
            public PersistenceState(
                Id.PersistenceState id, 
                List<Product.PersistenceState> products, 
                Client.PersistenceState client, 
                Option<AdditionalInformation.PersistenceState> additionalInformation, 
                DateTime creationDate)
            {
                Id = id;
                Products = products;
                Client = client;
                AdditionalInformation = additionalInformation;
                CreationDate = creationDate;
            }
        }
    }
    
}