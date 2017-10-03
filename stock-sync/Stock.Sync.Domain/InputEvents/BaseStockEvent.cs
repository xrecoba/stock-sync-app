using stock_sync;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.InputEvents
{
    abstract class BaseStockEvent
    {
        protected readonly ProductsRepository ProductsRepository;
        protected readonly int Id;

        protected BaseStockEvent(ProductsRepository productsRepository, int id)
        {
            ProductsRepository = productsRepository;
            Id = id;
        }

        protected Product GetProduct => ProductsRepository.GetProduct(Id);
    }
}