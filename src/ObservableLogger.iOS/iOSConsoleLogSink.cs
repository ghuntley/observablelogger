using System;

namespace ObservableLogger
{
    // ReSharper disable once InconsistentNaming
    public class iOSConsoleLogSink : DisposableBase
    {
        private readonly IDisposable _entriesSubscription;

        public iOSConsoleLogSink(ILoggerService loggerService)
        {
            _entriesSubscription = loggerService
                .Entries
                .Subscribe(WriteToiOSConsole);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _entriesSubscription.Dispose();
            }
        }

        private void WriteToiOSConsole(LogEntry logEntry)
        {
            var message =
                $"{logEntry.Timestamp.ToLocalTime():yyyy-MM-dd HH:mm:ss.fff} #{logEntry.ThreadId:00} [{logEntry.Level,-5}] {logEntry.Name} : {logEntry.Message}";

            Console.WriteLine(message);
         }
    }
}