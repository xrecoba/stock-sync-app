using System;
using System.Collections.Generic;
using System.Linq;

namespace stock_sync
{
    public class ParentProduct : Product
    {
        /// <summary>
        /// By default parent product is not ended, even if its stock is zero.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stock"></param>
        public ParentProduct(int id, int stock) : base(false, id, stock)
        {
            Children = new List<ChildProduct>();
        }

        public List<ChildProduct> Children { get; set; }

        public override IEnumerable<Product> GetOtherTreeProducts()
        {
            return Children;
        }

        public override bool IsParent() => true;
    }

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


    public class ChildProduct : Product
    {
        public ParentProduct ParentProduct { get; set; }
        public ChildProduct(int id, int stock, ParentProduct parent) : base(false, id, stock)
        {
            ParentProduct = parent;
        }

        public override IEnumerable<Product> GetOtherTreeProducts()
        {
            yield return ParentProduct;
            foreach (var sibling in ParentProduct.Children.Where(c => c != this))
            {
                yield return sibling;
            }
        }

        public override bool IsParent() => false;
    }
}
