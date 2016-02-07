using System;
using System.IO;
using System.Text;

namespace ObservableLogger
{
    public sealed class LogFileSink : DisposableBase
    {
        private readonly IDisposable _entriesSubscription;
        private readonly Func<Stream> _getInputStream;
        private readonly object _sync;
        private readonly StreamWriter _writer;

        public LogFileSink(ILoggerService loggerService, Stream outputStream, Func<Stream> getInputStream)
        {
            _sync = new object();
            _writer = new StreamWriter(outputStream, Encoding.UTF8);
            _getInputStream = getInputStream;

            _writer.WriteLine();
            _writer.WriteLine();

            _entriesSubscription = loggerService
                .Entries
                .Subscribe(WriteToLogFile);
        }

        public Stream GetLogFileStream() =>
            _getInputStream();

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _writer.Dispose();
                _entriesSubscription.Dispose();
            }
        }

        private void WriteToLogFile(LogEntry logEntry)
        {
            lock (_sync)
            {
                var message =
                    $"{logEntry.Timestamp.ToLocalTime():yyyy-MM-dd HH:mm:ss.fff} #{logEntry.ThreadId:00} [{logEntry.Level,-5}] {logEntry.Name} : {logEntry.Message}";

                _writer.WriteLine(message);
                _writer.Flush();
            }
        }
    }
}