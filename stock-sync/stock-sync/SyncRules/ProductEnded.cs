using stock_sync;
using Stock.Sync.Domain.InputEvents;

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
                    child.IsEnded = true;
                }
            }
        }
    }
}