using System.Collections.Generic;
using stock_sync;
using Stock.Sync.Domain.Execution;
using Stock.Sync.Domain.InputEvents;
using Stock.Sync.Domain.Repositories;
using Xunit;

namespace Stock.Sync.Tests.Unit.ExampleScenario
{
    public class Scenarios
    {
        internal const int ParentId = 1;
        internal const int Child1Id = 2;
        internal const int Child2Id = 3;

        internal static ProductsRepository GetDemoProductsRepo()
        {
            var repo = new ProductsRepository();
            new ProductCreated(repo, ParentId, 10, null).Apply();
            new ProductCreated(repo, Child1Id, 10, ParentId).Apply();
            new ProductCreated(repo, Child2Id, 10, ParentId).Apply();
            return repo;
        }

        [Fact]
        public void WhenPIsChangedTo9_C1andC2ShouldBeUpdatedTo9()
        {
            var repo = GetDemoProductsRepo();

            var executionEngine = new Engine();
            executionEngine.Run(repo, new List<IStockEvent> { new ProductUpdated(repo, ParentId, 9) });

            Assert.Equal(9, repo.GetProduct(Child1Id).Stock);
            Assert.Equal(9, repo.GetProduct(Child2Id).Stock);
        }

        [Fact]
        public void WhenC1StockBecomesZero_ThenParentAndC2Ended()
        {
            var repo = GetDemoProductsRepo();

            var executionEngine = new Engine();
            executionEngine.Run(repo, new List<IStockEvent> { new ProductUpdated(repo, Child1Id, 0) });

            Assert.Equal(true, repo.GetProduct(ParentId).IsEnded);
            Assert.Equal(true, repo.GetProduct(Child2Id).IsEnded);
        }

        [Fact]
        public void WhenParentIsEnded_ThenChildrenIsEnded()
        {
            var repo = GetDemoProductsRepo();

            var executionEngine = new Engine();
            executionEngine.Run(repo, new List<IStockEvent> { new ProductEnded(repo, ParentId) });

            Assert.Equal(true, repo.GetProduct(Child1Id).IsEnded);
            Assert.Equal(true, repo.GetProduct(Child2Id).IsEnded);
        }
    }
}