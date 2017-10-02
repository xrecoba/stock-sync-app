using stock_sync;
using Stock.Sync.Domain.InputEvents;

namespace Stock.Sync.Domain.OutputEvents
{
    class EndProduct : BaseOutputEvent
    {
        private readonly Product _product;

        public EndProduct(Product product)
        {
            _product = product;
        }

        public override void Apply()
        {
            _product.IsEnded = true;
        }

        internal override string ToJson()
        {
            return $"{{\'type\': \'EndProduct\', \'id\': {_product.Id}}}";
        }
    }
}