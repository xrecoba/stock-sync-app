using System;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Tests.Unit
{
    internal static class DemoData
    {
        internal static ProductsRepository GetDemoProductsRepo()
        {
            var repo = new ProductsRepository();
            new Domain.InputEvents.ProductCreated(repo, 1, 10, null).Apply();
            return repo;
        }
    }
}
