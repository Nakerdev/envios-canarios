using System;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Id
    {
        public Guid Value { get; }

        public static Id Create()
        {
            return new Id(Guid.NewGuid());
        }

        public Id(Guid value)
        {
            Value = value;
        }
    }
}