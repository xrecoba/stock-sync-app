using System;

namespace Stock.Sync.Domain.Execution
{
    public class ConsoleLogger : ILogger
    {
        public void LogMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}