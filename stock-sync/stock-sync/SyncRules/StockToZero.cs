using System.Linq;
using stock_sync;

namespace stock.sync.SyncRules
{
    class StockToZero
    {
        public void Apply(ChildProduct product)
        {
            product.ParentProduct.IsEnded = true;

            foreach (var siblings in product.ParentProduct.Children.Where(p => p != product))
            {
                siblings.IsEnded = true;
            }
        }
    }

    class StockUpdated
    {
        public void Apply(Product product, int stock)
        {
            //product.
        }
    }


    // After a product has been ended, is there any way to enable it again?

    class ChildEnded
    {
        
    }

    class ParentEnded
    {
        public void Apply(ParentProduct product)
        {
            foreach (var child in product.Children)
            {
                child.IsEnded = true;
            }
        }
    }
}