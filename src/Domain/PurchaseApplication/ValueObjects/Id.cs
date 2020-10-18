using System;
using LanguageExt;

namespace CanaryDeliveries.Domain.PurchaseApplication.ValueObjects
{
    public sealed class Id : Record<Id>
    {
        private Guid value;

        public static Id Create()
        {
            return new Id(Guid.NewGuid());
        }

        public Id(Guid value)
        {
            this.value = value;
        }
    }
}