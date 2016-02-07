namespace ObservableLogger
{
    public interface ILogger
    {
        string Name { get; }

        bool IsDebugEnabled { get; }

        bool IsInfoEnabled { get; }

        bool IsPerfEnabled { get; }

        bool IsWarnEnabled { get; }

        bool IsErrorEnabled { get; }

        void Log(LogLevel level, string message);
    }
}