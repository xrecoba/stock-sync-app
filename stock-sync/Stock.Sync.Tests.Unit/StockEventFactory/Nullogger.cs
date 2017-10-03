using Stock.Sync.Domain.Execution;

namespace Stock.Sync.Tests.Unit.StockEventFactory
{
    public class Nullogger : ILogger
    {
        public void LogMessage(string message)
        {
            
        }
    }
}