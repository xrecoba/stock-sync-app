using Stock.Sync.Domain.InputEvents;
using Xunit;

namespace Stock.Sync.Tests.Unit
{
    public class ProductUpdatedTests
    {
        [Fact]
        public void WhenUpdate_StockUpdated()
        {
            var repo = DemoData.GetDemoProductsRepo();
            var updatedProduct = repo.GetProduct(1);

            var originalStock = updatedProduct.Stock;
            new ProductUpdated(repo, updatedProduct.Id, originalStock - 1).Apply();
            Assert.Equal(originalStock -1, updatedProduct.Stock);
        }
    }
}