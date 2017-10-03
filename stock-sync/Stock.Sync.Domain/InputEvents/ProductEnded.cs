using System;
using System.Collections.Generic;
using System.Diagnostics;
using stock_sync;
using Stock.Sync.Domain.OutputEvents;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.InputEvents
{
    [DebuggerDisplay("Product ended: id-{Id}")]
    class ProductEnded : BaseStockEvent, IStockEvent
    {
        public ProductEnded(ProductsRepository productsRepository, int id) : base(productsRepository, id) { }
        
        public void Apply()
        {
            var product = GetProduct;
            if (product == null)
            {
                throw new ArgumentException($"Unexisting product {Id}");
            }
            product.IsEnded = true;
        }

        public IEnumerable<IStockEvent> GetSyncRules()
        {
            yield return new stock.sync.SyncRules.ProductEnded(GetProduct);
        }

        public IEnumerable<BaseOutputEvent> GetOutputEvents()
        {
            yield break;
        }
    }
}