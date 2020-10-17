using System;

namespace CanaryDeliveries.WebApp.Api.Logs
{
    public interface Logger
    {
        void LogError(Exception exception, string message);
    }
}