using System;
using Android.Util;

namespace ObservableLogger
{
    public class AndroidConsoleLogSink : DisposableBase
    {
        private readonly IDisposable _entriesSubscription;

        public AndroidConsoleLogSink(ILoggerService loggerService)
        {
            _entriesSubscription = loggerService
                .Entries
                .Subscribe(WriteToAndroidConsole);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _entriesSubscription.Dispose();
            }
        }

        private void WriteToAndroidConsole(LogEntry logEntry)
        {
            var message =
                $"{logEntry.Timestamp.ToLocalTime():yyyy-MM-dd HH:mm:ss.fff} #{logEntry.ThreadId:00} [{logEntry.Level,-5}] {logEntry.Name} : {logEntry.Message}";

            switch (logEntry.Level)
            {
                case LogLevel.Perf:
                case LogLevel.Debug:
                    Log.Debug(logEntry.Name, message);
                    break;
                case LogLevel.Info:
                    Log.Info(logEntry.Name, message);
                    break;
                case LogLevel.Warn:
                    Log.Warn(logEntry.Name, message);
                    break;
                case LogLevel.Error:
                    Log.Error(logEntry.Name, message);
                    break;
                default:
                    Log.Debug(logEntry.Name, message);
                    break;
            }
        }
    }
}