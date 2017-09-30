using System;
using System.Collections.Generic;
using System.Text;
using Stock.Sync.Domain.InputEvents;
using stock_sync;

namespace Stock.Sync.Domain.OutputEvents
{
    class UpdateProduct : BaseOutputEvent, IStockEvent
    {
        private readonly Product _otherProductInTree;
        private readonly int _newStock;

        public UpdateProduct(Product otherProductInTree, int newStock)
        {
            this._otherProductInTree = otherProductInTree;
            this._newStock = newStock;
        }

        public void Apply()
        {
            _otherProductInTree.Stock = _newStock;
        }
    }
}
