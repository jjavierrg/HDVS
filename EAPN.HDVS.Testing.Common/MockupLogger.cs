using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EAPN.HDVS.Testing.Common
{
    public class LogEntry
    {
        public LogLevel LogLevel { get; }
        public string Message { get; }
        public LogEntry(LogLevel logLevel, string message)
        {
            LogLevel = logLevel;
            Message = message;
        }
    }

    public class MockupLogger<T> : ILogger<T>
    {
        public IReadOnlyList<LogEntry> LogEntries => new List<LogEntry>(Logger.Instance.LogEntries).AsReadOnly();
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);
            Logger.Instance.LogEntries.Add(new LogEntry(logLevel, message));
        }
    }

    internal sealed class Logger
    {
        private Logger() { }
        public static Logger Instance { get; } = new Logger();
        public List<LogEntry> LogEntries = new List<LogEntry>();
    }
}
