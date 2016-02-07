using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableLogger
{
    // ReSharper disable once InconsistentNaming
    public class UWPConsoleLogSink : DisposableBase
    {
        private readonly IDisposable _entriesSubscription;

        public UWPConsoleLogSink(ILoggerService loggerService)
        {
            _entriesSubscription = loggerService
                .Entries
                .Subscribe(WriteToUWPConsole);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _entriesSubscription.Dispose();
            }
        }

        private void WriteToUWPConsole(LogEntry logEntry)
        {
            var message =
                $"{logEntry.Timestamp.ToLocalTime():yyyy-MM-dd HH:mm:ss.fff} #{logEntry.ThreadId:00} [{logEntry.Level,-5}] {logEntry.Name} : {logEntry.Message}";

            Debug.WriteLine(message);
        }
    }
}
