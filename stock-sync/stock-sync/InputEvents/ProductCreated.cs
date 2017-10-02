using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using stock_sync;
using Stock.Sync.Domain.OutputEvents;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.InputEvents
{
    [DebuggerDisplay("Product created: id-{Id} stock-{_stock} parent-{_parentId} ")]
    class ProductCreated : BaseStockEvent, IStockEvent
    {
        private readonly int _stock;
        private readonly int? _parentId;

        public ProductCreated(ProductsRepository productsRepository, int id, int stock, int? parentId) : base (productsRepository, id)
        {
            _stock = stock;
            _parentId = parentId;            
        }

        public void Apply()
        {
            Product product;
            if (!_parentId.HasValue)
            {
                product = new ParentProduct(Id, _stock);
            }
            else
            {
                var parent = (ParentProduct)ProductsRepository.GetProduct(_parentId.Value);
                if (parent == null)
                {
                    throw new ArgumentException($"Unexisting parentId {_parentId}");
                }
                else
                {
                    product = new ChildProduct(Id, _stock, parent);
                    parent.Children.Add((ChildProduct) product);
                }
            }
            ProductsRepository.AddProduct(product);
        }

        public IEnumerable<IStockEvent> GetSyncRules()
        {
            yield break;
        }

        public IEnumerable<BaseOutputEvent> GetOutputEvents()
        {
            yield break;
        }
    }
}
