using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using stock_sync;

namespace Stock.Sync.Domain.Repositories
{
    internal class ProductsRepository
    {
        private readonly IList<Product> _products;

        public ProductsRepository()
        {
            _products = new List<Product>();
        }

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public Product GetProduct(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }
    }
}
