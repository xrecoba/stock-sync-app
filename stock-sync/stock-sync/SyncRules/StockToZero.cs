using System;
using System.Linq;
using stock_sync;
using Stock.Sync.Domain.InputEvents;

namespace stock.sync.SyncRules
{
    class StockToZero : SyncEvent, IStockEvent
    {
        private readonly Product _product;

        public StockToZero(Product product)
        {
            _product = product;
        }

        public void Apply()
        {
            // Thanks to this method there's no need to have access to the products repository.
            // This only works because current structure is in memory, else we would have to update
            // the products affected by this sync rule via the repository.
            foreach (var otherProductInTree in _product.GetOtherTreeProducts())
            {
                otherProductInTree.IsEnded = true;
            }
        }
    }
}