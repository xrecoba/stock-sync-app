using System.Collections.Generic;
using stock.sync.SyncRules;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.InputEvents
{
    class ProductUpdated : BaseStockEvent, IStockEvent
    {
        private readonly int _stock;

        public ProductUpdated(ProductsRepository productsRepository, int id, int stock) : base(productsRepository, id)
        {            
            _stock = stock;
        }

        public void Apply()
        {
            GetProduct.Stock = _stock;
        }

        public IEnumerable<IStockEvent> GetSyncRules()
        {
            if (_stock == 0)
            {
                yield return new StockToZero(GetProduct);
            }
            else
            {
                yield return new StockUpdated(GetProduct);
            }
        }
    }
}