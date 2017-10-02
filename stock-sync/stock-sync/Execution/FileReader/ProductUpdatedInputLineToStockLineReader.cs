using Stock.Sync.Domain.InputEvents;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.Execution
{
    class ProductUpdatedInputLineToStockLineReader : IInputLineToStockEventAdapter
    {
        public IStockEvent GetInputEvent(ProductsRepository repository, InputLine inputLine)
        {
            return new ProductUpdated(repository, inputLine.id, inputLine.stock);
        }
    }
}