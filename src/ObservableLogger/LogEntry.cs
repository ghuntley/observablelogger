using System;

namespace ObservableLogger
{
    public struct LogEntry
    {
        public LogEntry(DateTimeOffset timestamp, string name, LogLevel level, int threadId, string message)
        {
            Timestamp = timestamp;
            Name = name;
            Level = level;
            ThreadId = threadId;
            Message = message;
        }

        public DateTimeOffset Timestamp { get; }

        public string Name { get; }

        public LogLevel Level { get; }

        public int ThreadId { get; }

        public string Message { get; }
    }
}