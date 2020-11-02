using System;
using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
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