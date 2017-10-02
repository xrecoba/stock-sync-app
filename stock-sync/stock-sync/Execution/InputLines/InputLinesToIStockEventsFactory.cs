using System;
using System.Collections.Generic;
using System.Linq;
using Stock.Sync.Domain.InputEvents;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.Execution
{
    class InputLinesToIStockEventsFactory
    {
        private readonly ProductsRepository _productsRepository;
        private readonly ILogger _logger;
        private readonly ProductCreatedInputLineToStockLineReader _productCreatedToStockLineReader;
        private readonly ProductUpdatedInputLineToStockLineReader _productUpdatedToStockLineReader;
        private readonly ProductEndedInputLineToStockLineReader _productEndedToStockLineReader;

        public InputLinesToIStockEventsFactory(ProductsRepository productsRepository, ILogger logger)
        {
            _productsRepository = productsRepository;
            _logger = logger;

            _productCreatedToStockLineReader = new ProductCreatedInputLineToStockLineReader();
            _productUpdatedToStockLineReader = new ProductUpdatedInputLineToStockLineReader();
            _productEndedToStockLineReader = new ProductEndedInputLineToStockLineReader();
        }


        public IEnumerable<IStockEvent> GetInputEvents(IEnumerable<InputLine> lines)
        {
            List<IStockEvent> inputEvents = new List<IStockEvent>(lines.Count()); 

            if (lines == null) throw new ArgumentNullException(nameof(lines));

            foreach (var inputLine in lines)
            {
                IInputLineToStockEventAdapter lineToStockEventAdapter = null;
                switch (inputLine.type)
                {
                    case Constants.ProductCreated:
                        lineToStockEventAdapter = _productCreatedToStockLineReader;
                        break;
                    case Constants.ProductUpdated:
                        lineToStockEventAdapter = _productUpdatedToStockLineReader;
                        break;
                    case Constants.ProductEnded:
                        lineToStockEventAdapter = _productEndedToStockLineReader;
                        break;
                    default:
                        _logger.LogMessage($"Unknown action type {inputLine.type}");
                        break;
                }
                if (lineToStockEventAdapter != null)
                {
                    try
                    {
                        inputEvents.Add(lineToStockEventAdapter.GetInputEvent(_productsRepository, inputLine));
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            // This addition should not be done here, it should be done in the engine instead but then we need a reference
            // to the products repository in the index. This would be fixed by using IOC and making the product repository 
            // a singleton accessible to everyone.
            inputEvents.Add(new OutputEvents.StockSummary(_productsRepository));
            return inputEvents;
        }

    }
}