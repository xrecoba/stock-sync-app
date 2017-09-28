using System.Collections.Generic;

namespace stock_sync
{
    public abstract class Product
    {
        public bool IsEnded { get; set; }
        public int Id { get; }
        public int Stock { get; set; }

        public Product(bool isEnded, int id, int stock)
        {
            IsEnded = isEnded;
            Id = id;
            Stock = stock;
        }

        public abstract IEnumerable<Product> GetOtherTreeProducts();
        public abstract bool IsParent();

    }
}