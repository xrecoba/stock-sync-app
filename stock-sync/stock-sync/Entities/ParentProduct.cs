using System;
using System.Collections.Generic;

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
}
