using System;
using LanguageExt;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class Id : Record<Id>
    {
        public PersistenceState State => new PersistenceState(value.ToString());
        
        private readonly Guid value;

        public static Id Create()
        {
            return new Id(Guid.NewGuid());
        }

        public Id(PersistenceState persistenceState)
        {
            value = new Guid(persistenceState.Value);
        }

        private Id(Guid value)
        {
            this.value = value;
        }

        public sealed class PersistenceState
        {
            public string Value { get; }

            public PersistenceState(string value)
            {
                Value = value;
            }
        }
    }
}