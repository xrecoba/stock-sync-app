using System;
using System.Collections.Generic;
using System.Diagnostics;
using stock.sync.SyncRules;
using Stock.Sync.Domain.OutputEvents;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.InputEvents
{
    [DebuggerDisplay("Product updated: id-{Id} stock-{_stock}")]
    class ProductUpdated : BaseStockEvent, IStockEvent
    {
        private readonly int _stock;

        public ProductUpdated(ProductsRepository productsRepository, int id, int stock) : base(productsRepository, id)
        {            
            _stock = stock;
        }

        public void Apply()
        {
            var product = GetProduct;
            if (product == null)
            {
                throw new ArgumentException($"Unexisting product {Id}");
            }
            product.Stock = _stock;
        }

        public IEnumerable<IStockEvent> GetSyncRules()
        {
            if (_stock == 0)
            {
                yield return new StockToZero(GetProduct);
            }
            else
            {
                yield return new StockUpdated(GetProduct);
            }
        }

        public IEnumerable<BaseOutputEvent> GetOutputEvents()
        {
            yield break;
        }
    }
}