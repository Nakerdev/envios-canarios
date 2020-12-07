using System;

namespace CanaryDeliveries.Common.Logging
{
    public interface Logger
    {
        void LogError(Exception exception, string message);
    }
}