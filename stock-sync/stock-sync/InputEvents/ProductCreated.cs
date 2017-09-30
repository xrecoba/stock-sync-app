using System;
using System.Collections.Generic;
using System.Text;
using stock_sync;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.InputEvents
{
    class ProductCreated : IStockEvent
    {
        private readonly ProductsRepository _productsRepository;
        private readonly int _id;
        private readonly int _stock;
        private readonly int? _parentId;

        public ProductCreated(ProductsRepository productsRepository, int id, int stock, int? parentId)
        {
            _productsRepository = productsRepository;
            _id = id;
            _stock = stock;
            _parentId = parentId;            
        }

        public void Apply()
        {
            Product product;
            if (!_parentId.HasValue)
            {
                product = new ParentProduct(_id, _stock);
            }
            else
            {
                var parent = (ParentProduct)_productsRepository.GetProduct(_parentId.Value);                
                product = new ChildProduct(_id, _stock, parent);
                parent.Children.Add((ChildProduct) product);
            }
            _productsRepository.AddProduct(product);
        }

        public IEnumerable<IStockEvent> GetSyncRules()
        {
            yield break;
        }

        public IEnumerable<IStockEvent> GetOutputEvents()
        {
            yield break;
        }
    }
}
