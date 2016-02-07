using PCLMock;

namespace ObservableLogger.Tests
{
    public sealed partial class LoggerMock
    {
        partial void ConfigureLooseBehavior()
        {
            this
                .When(x => x.IsDebugEnabled)
                .Return(true);
            this
                .When(x => x.IsErrorEnabled)
                .Return(true);
            this
                .When(x => x.IsInfoEnabled)
                .Return(true);
            this
                .When(x => x.IsPerfEnabled)
                .Return(true);
            this
                .When(x => x.IsWarnEnabled)
                .Return(true);
        }
    }

    public partial class LoggerMock : MockBase<ILogger>, ILogger
    {
        public LoggerMock(MockBehavior behavior = MockBehavior.Strict) : base(behavior)
        {
            if (behavior == MockBehavior.Loose)
            {
                ConfigureLooseBehavior();
            }
        }

        public string Name
        {
            get { return Apply(x => x.Name); }
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

        public void Log(LogLevel level, string message)
        {
            Apply(x => x.Log(level, message));
        }

        partial void ConfigureLooseBehavior();
    }

}