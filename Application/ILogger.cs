using System;

namespace Core.Application
{
    public interface ILogger
    {
        void Log(string message);
        void LogError(Exception ex, string message);
    }
}
