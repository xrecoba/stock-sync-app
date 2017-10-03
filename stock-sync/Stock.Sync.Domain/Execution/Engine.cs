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
    public class Engine
    {
        readonly ILogger _errorsLogger;
        readonly ILogger _outputLogger;


        public Engine() :  this(new ConsoleLogger(), new ConsoleLogger()) { }

        public Engine(ILogger errorsLogger, ILogger outputLogger)
        {
            _errorsLogger = errorsLogger;
            _outputLogger = outputLogger;
        }

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
                                _outputLogger.LogMessage(outputEvent.ToJson());
                            } 
                        }
                    } 
                }
            }

            _outputLogger.LogMessage(new StockSummary(productsRepository).ToJson());
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
                _errorsLogger.LogMessage(e.Message);
                return false;
            }
        }
    }
}
