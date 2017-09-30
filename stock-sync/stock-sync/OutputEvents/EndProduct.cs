using stock_sync;
using Stock.Sync.Domain.InputEvents;

namespace Stock.Sync.Domain.OutputEvents
{
    class EndProduct : BaseOutputEvent, IStockEvent
    {
        private readonly Product _product;

        public EndProduct(Product product)
        {
            _product = product;
        }

        public void Apply()
        {
            _product.IsEnded = true;
        }
    }
}