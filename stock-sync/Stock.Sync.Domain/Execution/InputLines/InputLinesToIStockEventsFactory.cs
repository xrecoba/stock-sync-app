using System;
using System.Collections.Generic;
using System.Linq;
using Stock.Sync.Domain.InputEvents;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.Execution
{
    public class InputLinesToIStockEventsFactory
    {
        private readonly ProductsRepository _productsRepository;
        private readonly ILogger _logger;
        private readonly ProductCreatedInputLineToStockLineReader _productCreatedToStockLineReader;
        private readonly ProductUpdatedInputLineToStockLineReader _productUpdatedToStockLineReader;
        private readonly ProductEndedInputLineToStockLineReader _productEndedToStockLineReader;

        public InputLinesToIStockEventsFactory(ProductsRepository productsRepository) : this(productsRepository,
            new ConsoleLogger())
        {
        }

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
                        // thanks to parameter validation this will never be reached.                        
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
                        _logger.LogMessage(e.Message);
                    }
                }
            }

            return inputEvents;
        }        
    }
}