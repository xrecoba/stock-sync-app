using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Stock.Sync.Domain.InputEvents;
using Stock.Sync.Domain.OutputEvents;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.Execution
{
    internal class Engine
    {
        ILogger _logger = new ConsoleLogger();
        private int _currentTimestamp;

        public void Run(ProductsRepository productsRepository, IEnumerable<IStockEvent> events)
        {
            foreach (var stockEvent in events)
            {
                if (TryExecute(stockEvent))
                {
                    foreach (var syncRule in stockEvent.GetSyncRules())
                    {
                        if (TryExecute(syncRule))
                        {
                            foreach (var outputEvent in syncRule.GetOutputEvents())
                            {
                                TryExecute(outputEvent);
                                Console.WriteLine(outputEvent.ToJson());
                            } 
                        }
                    } 
                }
            }

            Console.WriteLine(new StockSummary(productsRepository).ToJson());
        }

        private bool TryExecute(IStockEvent stockEvent)
        {
            try
            {
                stockEvent.Apply();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogMessage(e.Message);
                return false;
            }
        }
    }
}
