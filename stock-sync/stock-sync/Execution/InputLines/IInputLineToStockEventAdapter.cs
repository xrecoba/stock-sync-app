using Stock.Sync.Domain.InputEvents;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.Execution
{
    interface IInputLineToStockEventAdapter
    {
        IStockEvent GetInputEvent(ProductsRepository repository, InputLine inputLine);
    }
}