using System;
using System.Collections.Generic;
using stock_sync;
using Stock.Sync.Domain.InputEvents;
using Stock.Sync.Domain.OutputEvents;

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
                OutputEvents.Add(new UpdateProduct(otherProductInTree, _product.Stock));
            }
        }
    }
}