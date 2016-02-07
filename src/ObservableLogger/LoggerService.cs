using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ObservableLogger
{
    public sealed class LoggerService : ILoggerService
    {
        private readonly ISubject<LogEntry> _entries;
        private readonly IDictionary<string, ILogger> _loggers;
        private readonly object _sync;

        public LoggerService()
        {
            _loggers = new Dictionary<string, ILogger>();
            _entries = new ReplaySubject<LogEntry>(64);
            _sync = new object();
        }

        public LogLevel Threshold { get; set; }

        public bool IsDebugEnabled => Threshold <= LogLevel.Debug;

        public bool IsInfoEnabled => Threshold <= LogLevel.Info;

        public bool IsPerfEnabled => Threshold <= LogLevel.Perf;

        public bool IsWarnEnabled => Threshold <= LogLevel.Warn;

        public bool IsErrorEnabled => Threshold <= LogLevel.Error;

        public IObservable<LogEntry> Entries => _entries.Where(x => x.Level >= Threshold);

        public ILogger GetLogger(Type forType)
        {
            Ensure.ArgumentNotNull(forType, nameof(forType));

            if (forType.IsConstructedGenericType)
            {
                forType = forType.GetGenericTypeDefinition();
            }

            return GetLogger(forType.FullName);
        }

        public ILogger GetLogger(string name)
        {
            Ensure.ArgumentNotNull(name, nameof(name));

            lock (_sync)
            {
                ILogger logger;

                if (!_loggers.TryGetValue(name, out logger))
                {
                    logger = new Logger(this, name);
                    _loggers.Add(name, logger);
                }

                return logger;
            }
        }

        private sealed class Logger : ILogger
        {
            private readonly LoggerService owner;

            public Logger(LoggerService owner, string name)
            {
                this.owner = owner;
                Name = name;
            }

            public string Name { get; }

            public bool IsDebugEnabled => owner.IsDebugEnabled;

            public bool IsInfoEnabled => owner.IsInfoEnabled;

            public bool IsPerfEnabled => owner.IsPerfEnabled;

            public bool IsWarnEnabled => owner.IsWarnEnabled;

            public bool IsErrorEnabled => owner.IsErrorEnabled;

            public void Log(LogLevel level, string message)
            {
                var entry = new LogEntry(DateTime.UtcNow, Name, level, Environment.CurrentManagedThreadId, message);
                owner._entries.OnNext(entry);
            }
        }
    }
}