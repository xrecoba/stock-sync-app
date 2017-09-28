using System;
using System.Collections.Generic;
using System.Text;
using stock_sync;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.InputEvents
{
    class ProductCreated
    {
        public ProductCreated(ProductsRepository productsRepository, int id, int stock, int? parentId)
        {
            Product product;
            if (!parentId.HasValue)
            {
                product = new ParentProduct(id, stock);
            }
            else
            {
                var parent = (ParentProduct) productsRepository.GetProduct(parentId.Value);
                product = new ChildProduct(id, stock, parent);
            }
            productsRepository.AddProduct(product);
        }
    }

    class ProductUpdated
    {
        public ProductUpdated(ProductsRepository productsRepository, int id, int stock)
        {
            productsRepository.GetProduct(id).Stock = stock;
        }
    }

    class ProductEnded
    {
        public ProductEnded(ProductsRepository productsRepository, int id)
        {
            productsRepository.GetProduct(id).IsEnded = true;
        }
    }
}
