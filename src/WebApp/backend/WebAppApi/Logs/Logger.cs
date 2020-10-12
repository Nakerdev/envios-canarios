using System;

namespace WebAppApi.Logs
{
    public interface Logger
    {
        void LogError(Exception exception, string message);
    }
}