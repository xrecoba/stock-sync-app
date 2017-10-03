using System;
using System.IO;

namespace Stock.Sync.Domain.Execution
{
    public class FileLogger : ILogger, IDisposable
    {
        private readonly StreamWriter _streamWriter;

        public FileLogger(string file)
        {
            var logFile = File.Create(file);
            _streamWriter = new StreamWriter(logFile);
        }

        public void LogMessage(string message)
        {
            _streamWriter.WriteLine(message);
        }

        public void Dispose()
        {
            _streamWriter?.Dispose();
        }
    }
}