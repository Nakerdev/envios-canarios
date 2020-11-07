using System;
using LanguageExt;

namespace CanaryDeliveries.PurchaseApplication.Domain.ValueObjects
{
    public sealed class Id : Record<Id>
    {
        private readonly Guid Value;

        public static Id Create()
        {
            return new Id(Guid.NewGuid());
        }

        private Id(Guid value)
        {
            Value = value;
        }
    }
}