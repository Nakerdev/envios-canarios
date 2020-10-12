using System;

namespace Architecture.Logs
{
    public interface Logger
    {
        void LogError(Exception exception, string message);
    }
}