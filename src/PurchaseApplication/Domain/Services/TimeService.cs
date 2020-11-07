using System;

namespace CanaryDeliveries.PurchaseApplication.Domain.Services
{
    public interface TimeService
    {
        DateTime UtcNow();
    }

    public sealed class SystemTimeService : TimeService
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}