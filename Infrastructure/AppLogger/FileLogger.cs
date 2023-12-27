using Core.Application;
using System;
using System.IO;
using System.Text;

namespace Infrastructure.AppLogger
{
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;

        public FileLogger(string filePath = "Logs\\log.txt")
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _logFilePath = Path.Combine(baseDirectory, filePath);

            string logDirectory = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

        }

        public void Log(string logEntry)
        {
            try
            {
                File.AppendAllText(_logFilePath, $"{logEntry}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }

        public void LogError(Exception ex, string message)
        {
            StringBuilder logEntry = new StringBuilder();
            logEntry.AppendLine("--------------------------------------------------")
                     .AppendLine($"Timestamp: {DateTime.Now}")
                     .AppendLine($"[ERROR] : {message}")
                     .AppendLine($"Exception Type: {ex.GetType().FullName}")
                     .AppendLine($"Exception Message: {ex.Message}")
                     .AppendLine($"Inner Exception: {ex.InnerException?.ToString() ?? "N/A"}")
                     .AppendLine($"Stack Trace: {ex.StackTrace}");
            logEntry.AppendLine("--------------------------------------------------");

            Log(logEntry.ToString());
        }
    }
}
