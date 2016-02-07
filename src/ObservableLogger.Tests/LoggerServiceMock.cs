using System;
using System.Reactive.Linq;
using PCLMock;

namespace ObservableLogger.Tests
{
    public sealed partial class LoggerServiceMock
    {
        partial void ConfigureLooseBehavior()
        {
            When(x => x.GetLogger(It.IsAny<Type>()))
                .Return(new LoggerMock(MockBehavior.Loose));
            When(x => x.GetLogger(It.IsAny<string>()))
                .Return(new LoggerMock(MockBehavior.Loose));
            When(x => x.Entries).Return(Observable.Empty<LogEntry>());
        }
    }

    public partial class LoggerServiceMock : MockBase<ILoggerService>, ILoggerService
    {
        public LoggerServiceMock(MockBehavior behavior = MockBehavior.Strict)
            : base(behavior)
        {
            if (behavior == MockBehavior.Loose)
            {
                ConfigureLooseBehavior();
            }
        }

        public LogLevel Threshold
        {
            get { return Apply(x => x.Threshold); }

            set { ApplyPropertySet(x => x.Threshold, value); }
        }

        public bool IsDebugEnabled
        {
            get { return Apply(x => x.IsDebugEnabled); }
        }

        public bool IsInfoEnabled
        {
            get { return Apply(x => x.IsInfoEnabled); }
        }

        public bool IsPerfEnabled
        {
            get { return Apply(x => x.IsPerfEnabled); }
        }

        public bool IsWarnEnabled
        {
            get { return Apply(x => x.IsWarnEnabled); }
        }

        public bool IsErrorEnabled
        {
            get { return Apply(x => x.IsErrorEnabled); }
        }

        public IObservable<LogEntry> Entries
        {
            get { return Apply(x => x.Entries); }
        }

        public ILogger GetLogger(Type forType)
        {
            return Apply(x => x.GetLogger(forType));
        }

        public ILogger GetLogger(string name)
        {
            return Apply(x => x.GetLogger(name));
        }

        partial void ConfigureLooseBehavior();
    }
}