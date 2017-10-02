using Stock.Sync.Domain.InputEvents;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.Execution
{
    class ProductEndedInputLineToStockLineReader : IInputLineToStockEventAdapter
    {
        public IStockEvent GetInputEvent(ProductsRepository repository, InputLine inputLine)
        {
            return new ProductEnded(repository, inputLine.id);
        }
    }
}