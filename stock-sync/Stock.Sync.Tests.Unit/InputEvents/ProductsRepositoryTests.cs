using stock_sync;
using Stock.Sync.Domain.Repositories;
using Xunit;

namespace Stock.Sync.Tests.Unit
{
    public class ProductsRepositoryTests
    {
        public class TheAddProductMethod
        {
            [Fact]
            public void WhenProductAdded_ThenItCanBeReturned()
            {
                var sut = new ProductsRepository();
                var addedProduct = new ParentProduct(3, 5);
                sut.AddProduct(addedProduct);

                var retrievedProduct = sut.GetProduct(3);
                Assert.Equal(addedProduct, retrievedProduct);
            }
        }

        public class TheGetProductMethod
        {
            [Fact]
            public void WhenUnexistingProduct_ReturnsNull()
            {
                var sut = new ProductsRepository();
                
                var retrievedProduct = sut.GetProduct(3);
                Assert.Equal(null, retrievedProduct);
            }
        }
    }
}