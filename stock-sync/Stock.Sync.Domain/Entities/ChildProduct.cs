using System.Collections.Generic;
using System.Linq;

namespace stock_sync
{
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