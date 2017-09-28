using System;
using stock_sync;
using Stock.Sync.Domain.InputEvents;
using Stock.Sync.Domain.Repositories;
using Xunit;

namespace Stock.Sync.Tests.Unit
{
    internal static class DemoData
    {
 internal static ProductsRepository GetDemoProductsRepo()
        {
            var repo = new ProductsRepository();
            new Stock.Sync.Domain.InputEvents.ProductCreated(repo, 1, 10, null);
            return repo;
        }
}

    public class ProductCreatedTests
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

            new Stock.Sync.Domain.InputEvents.ProductCreated(repo, parentId, anyStock, null);
            new Stock.Sync.Domain.InputEvents.ProductCreated(repo, childProductId, anyStock, parentId);

            var createdProduct = repo.GetProduct(childProductId) as ChildProduct;

            Assert.Equal(childProductId, createdProduct.Id);
            Assert.Equal(parentId, createdProduct.ParentProduct.Id);
        }
    }

    public class ProductUpdatedTests
    {
        [Fact]
        public void WhenUpdate_StockUpdated()
        {
            var repo = DemoData.GetDemoProductsRepo();
            var updatedProduct = repo.GetProduct(1);

            var originalStock = updatedProduct.Stock;
            var sut = new ProductUpdated(repo, updatedProduct.Id, originalStock - 1);
            Assert.Equal(originalStock -1, updatedProduct.Stock);
        }
    }

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
