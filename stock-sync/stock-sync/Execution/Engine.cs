using System;
using System.Collections.Generic;
using System.Text;
using Stock.Sync.Domain.InputEvents;

namespace Stock.Sync.Domain.Execution
{
    internal class Engine
    {
        public void Run(IEnumerable<IStockEvent> events)
        {
            foreach (var stockEvent in events)
            {
                stockEvent.Apply();
                foreach (var syncRule in stockEvent.GetSyncRules())
                {
                    syncRule.Apply();
                    foreach (var outputEvent in syncRule.GetOutputEvents())
                    {
                        outputEvent.Apply();
                        Console.WriteLine(outputEvent);
                        
                    }
                }
            }
        }
    }
}
