using System.Collections.Generic;
using stock_sync;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.InputEvents
{
    class ProductEnded : BaseStockEvent, IStockEvent
    {
        public ProductEnded(ProductsRepository productsRepository, int id) : base(productsRepository, id) { }
        
        public void Apply()
        {
            GetProduct.IsEnded = true;
        }

        public IEnumerable<IStockEvent> GetSyncRules()
        {
            yield return new stock.sync.SyncRules.ProductEnded(GetProduct);
        }

        public IEnumerable<IStockEvent> GetOutputEvents()
        {
            yield break;
        }
    }
}