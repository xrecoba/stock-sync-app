using stock_sync;
using Stock.Sync.Domain.Repositories;
using Xunit;

namespace Stock.Sync.Tests.Unit
{
    public class Scenarios
    {
        [Fact]
        public void WhenParentProductCreated_NewInstanceProvidedToRepository()
        {
            var repo = DemoData.GetDemoProductsRepo();
            var createdProduct = repo.GetProduct(1);

            Assert.Equal(1, createdProduct.Id);
        }

        

        [Fact]
        public void WhenChildProductCreated_NewInstanceWithParentProvidedToRepository()
        {
            var repo = new ProductsRepository();
            var parentId = 1;
            var childProductId = 12;
            var anyStock = 10;

            new Domain.InputEvents.ProductCreated(repo, parentId, anyStock, null).Apply();
            new Domain.InputEvents.ProductCreated(repo, childProductId, anyStock, parentId).Apply();

            var createdProduct = repo.GetProduct(childProductId) as ChildProduct;

            Assert.Equal(childProductId, createdProduct.Id);
            Assert.Equal(parentId, createdProduct.ParentProduct.Id);
        }
    }
}