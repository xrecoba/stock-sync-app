using System;
using stock_sync;
using Stock.Sync.Domain.InputEvents;

namespace stock.sync.SyncRules
{
    class StockUpdated : SyncEvent, IStockEvent
    {
        private readonly Product _product;

        public StockUpdated(Product product)
        {
            _product = product;
        }
        public void Apply()
        {
            foreach (var otherProductInTree in _product.GetOtherTreeProducts())
            {
                otherProductInTree.Stock = _product.Stock;                
            }
        }
    }
}