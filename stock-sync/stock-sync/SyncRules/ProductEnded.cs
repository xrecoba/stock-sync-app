using System.Collections.Generic;
using stock_sync;
using Stock.Sync.Domain.InputEvents;
using Stock.Sync.Domain.OutputEvents;

namespace stock.sync.SyncRules
{
    /// TODO - FUNCTIONAL QUESTION - After a product has been ended, is there any way to enable it again?
    class ProductEnded : SyncEvent, IStockEvent
    {
        private readonly Product _product;

        public ProductEnded(Product product)
        {
            _product = product;        
        }

        public void Apply()
        {
            if (_product.IsParent())
            {
                foreach (var child in _product.GetOtherTreeProducts())
                {
                    OutputEvents.Add(new EndProduct(child));
                }
            }
        }
    }
}