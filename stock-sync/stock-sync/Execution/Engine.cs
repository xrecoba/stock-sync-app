using System;
using System.Collections.Generic;
using System.Text;
using Stock.Sync.Domain.InputEvents;

namespace Stock.Sync.Domain.Execution
{
    internal class Engine
    {
        ILogger _logger = new ConsoleLogger(); 

        public void Run(IEnumerable<IStockEvent> events)
        {
            foreach (var stockEvent in events)
            {
                TryExecute(stockEvent);

                foreach (var syncRule in stockEvent.GetSyncRules())
                {
                    TryExecute(syncRule);

                    foreach (var outputEvent in syncRule.GetOutputEvents())
                    {
                        TryExecute(outputEvent);
                        Console.WriteLine(outputEvent.ToJson());
                    }
                }
            }
        }

        private void TryExecute(IStockEvent stockEvent)
        {
            try
            {
                stockEvent.Apply();
            }
            catch (Exception e)
            {
                _logger.LogMessage(e.Message);
            }
        }
    }
}
