using System;
using System.Collections.Generic;
using System.Text;
using Stock.Sync.Domain.InputEvents;
using stock_sync;

namespace Stock.Sync.Domain.OutputEvents
{
    class UpdateProduct : BaseOutputEvent
    {
        private readonly Product _product;
        private readonly int _newStock;

        public UpdateProduct(Product product, int newStock)
        {
            _product = product;
            _newStock = newStock;
        }

        public override void Apply()
        {
            _product.Stock = _newStock;
        }

        internal override string ToJson()
        {
            return $"{{\'type\': \'UpdateProduct\', \'id\': {_product.Id}, \'stock\': {_product.Stock}}}";
        }
    }
}
